package main

import (
	"context"
	"converter-backend/internal/config"
	"converter-backend/internal/handlers"
	"converter-backend/internal/logger"
	"converter-backend/internal/middleware"
	"converter-backend/internal/models"
	"converter-backend/internal/services"
	"converter-backend/internal/utils"
	"encoding/csv"
	"fmt"
	"io"
	"net/http"
	"os"
	"os/signal"
	"path/filepath"
	"strings"
	"syscall"
	"time"

	"github.com/gin-contrib/cors"
	"github.com/gin-gonic/gin"
	"github.com/xuri/excelize/v2"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

func main() {
	// Initialize logger
	logger.InitLogger()
	logger.Info("Starting application...")

	// Load configuration
	cfg, err := config.LoadConfig()
	if err != nil {
		logger.Error(fmt.Sprintf("Failed to load configuration: %v", err))
		os.Exit(1)
	}
	logger.Info("Configuration loaded successfully")

	if err := handlers.SetJWTSecret(cfg.JWTSecret); err != nil {
		logger.Error(fmt.Sprintf("Failed to configure JWT secret: %v", err))
		os.Exit(1)
	}

	// Initialize database
	db, err := initDB(cfg.DBConfig)
	if err != nil {
		logger.Error(fmt.Sprintf("Failed to initialize database: %v", err))
		os.Exit(1)
	}
	logger.Info("Database initialized successfully")

	// Initialize services
	emailService := services.NewEmailService(&cfg.Email)
	spreadsheetService := services.NewSpreadsheetService(cfg.DBConfig.DSN)

	// Initialize handlers
	authHandler := handlers.NewAuthHandlerWithEmail(db, emailService)
	fileHandler := handlers.NewFileHandler(spreadsheetService)
	aiHandler := handlers.NewAIHandlerWithDB(db)

	// Initialize rate limiters
	rateLimiter := middleware.NewRateLimiter(cfg.RateLimit.RequestsPerMinute)
	authRateLimiter := middleware.NewAuthRateLimiter() // Stricter rate limiter for auth endpoints

	// Setup router
	r := gin.Default()

	// CORS configuration
	r.Use(cors.New(cors.Config{
		AllowOrigins:     cfg.AllowedOrigins,
		AllowMethods:     []string{"GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH"},
		AllowHeaders:     []string{"Origin", "Content-Type", "Authorization", "X-API-Key"},
		ExposeHeaders:    []string{"Content-Length", "Content-Disposition", "X-Db-Saved", "X-RateLimit-Limit", "X-RateLimit-Remaining", "X-RateLimit-Reset"},
		AllowCredentials: true,
		MaxAge:           12 * time.Hour,
	}))

	// Apply rate limiting to all routes
	r.Use(rateLimiter.Middleware())

	// Health check endpoint (no auth, no rate limit bypass but very light)
	r.GET("/health", func(c *gin.Context) {
		c.JSON(http.StatusOK, gin.H{"status": "healthy"})
	})

	// API v1 routes
	v1 := r.Group("/api/v1")
	{
		// Public auth routes with stricter rate limiting
		authRoutes := v1.Group("")
		authRoutes.Use(authRateLimiter.Middleware())
		{
			authRoutes.POST("/register", authHandler.Register)
			authRoutes.POST("/login", authHandler.Login)
			authRoutes.POST("/forgot-password", authHandler.ForgotPassword)
			authRoutes.POST("/reset-password", authHandler.ResetPassword)
		}

		// Email verification without strict rate limiting (users may need to retry)
		v1.GET("/verify-email", authHandler.VerifyEmail)

		// Protected routes
		protected := v1.Group("")
		protected.Use(authHandler.AuthMiddleware())
		{
			protected.GET("/me", authHandler.Me)
			protected.POST("/api-key/generate", authHandler.GenerateAPIKey)
			protected.POST("/realtime/token", authHandler.GenerateRealtimeToken)

			// File management
			protected.GET("/files", fileHandler.List)
			protected.POST("/files", fileHandler.Save)
			protected.GET("/files/:id", fileHandler.Get)
			protected.DELETE("/files/:id", fileHandler.Delete)
			protected.GET("/files/:id/cells", fileHandler.GetCells)
			protected.PATCH("/files/:id/cells", fileHandler.PatchCells)
			protected.GET("/files/:id/schema", fileHandler.GetSchema)
			protected.POST("/files/:id/realtime/token", fileHandler.FileRealtimeToken)
			protected.GET("/files/:id/shares", fileHandler.ListShares)
			protected.POST("/files/:id/shares", fileHandler.CreateShare)
			protected.DELETE("/files/:id/shares/:userId", fileHandler.DeleteShare)

			// AI endpoints
			protected.GET("/ai/gemini-key", aiHandler.GetGeminiAPIKey)
			protected.POST("/ai/gemini-key", aiHandler.SetGeminiAPIKey)
			protected.POST("/ai/generate", aiHandler.GenerateAI)
			protected.POST("/ai/stream", aiHandler.StreamAI)
		}
	}

	// Legacy routes for backward compatibility (redirect to v1)
	legacyAuthRoutes := r.Group("/api")
	legacyAuthRoutes.Use(authRateLimiter.Middleware())
	{
		legacyAuthRoutes.POST("/register", authHandler.Register)
		legacyAuthRoutes.POST("/login", authHandler.Login)
	}

	legacyProtected := r.Group("/api")
	legacyProtected.Use(authHandler.AuthMiddleware())
	{
		legacyProtected.GET("/me", authHandler.Me)
		legacyProtected.GET("/files", fileHandler.List)
		legacyProtected.POST("/files", fileHandler.Save)
		legacyProtected.GET("/files/:id", fileHandler.Get)
		legacyProtected.DELETE("/files/:id", fileHandler.Delete)
		legacyProtected.POST("/realtime/token", authHandler.GenerateRealtimeToken)
		legacyProtected.GET("/files/:id/cells", fileHandler.GetCells)
		legacyProtected.PATCH("/files/:id/cells", fileHandler.PatchCells)
		legacyProtected.POST("/files/:id/realtime/token", fileHandler.FileRealtimeToken)
		legacyProtected.GET("/files/:id/schema", fileHandler.GetSchema)
		legacyProtected.GET("/files/:id/shares", fileHandler.ListShares)
		legacyProtected.POST("/files/:id/shares", fileHandler.CreateShare)
		legacyProtected.DELETE("/files/:id/shares/:userId", fileHandler.DeleteShare)
	}

	// File conversion endpoint (can be used with or without auth)
	r.POST("/convert", func(c *gin.Context) {
		// Check file size before processing
		c.Request.Body = http.MaxBytesReader(c.Writer, c.Request.Body, cfg.FileUpload.MaxSizeMB*1024*1024)

		file, header, err := c.Request.FormFile("file")
		if err != nil {
			if err.Error() == "http: request body too large" {
				c.JSON(http.StatusRequestEntityTooLarge, gin.H{"error": fmt.Sprintf("File too large. Maximum size is %dMB", cfg.FileUpload.MaxSizeMB)})
				return
			}
			c.JSON(http.StatusBadRequest, gin.H{"error": "No file uploaded"})
			return
		}
		defer file.Close()

		// Validate file type
		if err := utils.ValidateFile(file); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid file type. Only Excel (.xlsx, .xls) and CSV files are allowed"})
			return
		}

		filename := filepath.Base(header.Filename)

		// Save to temp file
		tmpFile, err := os.CreateTemp("", "upload-*.xlsx")
		if err != nil {
			logger.Error(fmt.Sprintf("Failed to create temp file: %v", err))
			c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to process file"})
			return
		}
		defer os.Remove(tmpFile.Name())

		// Reset file pointer and copy
		file.Seek(0, 0)
		if _, err := tmpFile.ReadFrom(file); err != nil {
			logger.Error(fmt.Sprintf("Failed to save temp file: %v", err))
			c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to process file"})
			return
		}
		tmpFile.Close()

		ext := strings.ToLower(filepath.Ext(filename))
		var rows [][]string

		if ext == ".csv" {
			rows, err = readCSVRows(tmpFile.Name())
			if err != nil {
				logger.Error(fmt.Sprintf("Failed to read CSV file: %v", err))
				c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to parse CSV file"})
				return
			}
		} else {
			// Open Excel file
			f, err := excelize.OpenFile(tmpFile.Name())
			if err != nil {
				logger.Error(fmt.Sprintf("Failed to open Excel file: %v", err))
				c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to parse Excel file"})
				return
			}
			defer f.Close()

			// Get rows
			sheetName := f.GetSheetName(0)
			rows, err = f.GetRows(sheetName)
			if err != nil {
				logger.Error(fmt.Sprintf("Failed to read rows: %v", err))
				c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to read Excel data"})
				return
			}
		}

		// Set headers for CSV download
		c.Header("Content-Description", "File Transfer")
		c.Header("Content-Transfer-Encoding", "binary")
		c.Header("Content-Disposition", fmt.Sprintf("attachment; filename=%s.csv", filename))
		c.Header("Content-Type", "text/csv")
		c.Header("X-Db-Saved", "true")

		// Stream CSV response
		writer := csv.NewWriter(c.Writer)
		for _, row := range rows {
			if err := writer.Write(row); err != nil {
				logger.Error(fmt.Sprintf("Error writing CSV row: %v", err))
				return
			}
		}
		writer.Flush()

		logger.Info(fmt.Sprintf("Successfully converted file: %s (%d rows)", filename, len(rows)))
	})

	// Create HTTP server
	srv := &http.Server{
		Addr:    ":" + cfg.ServerConfig.Port,
		Handler: r,
	}

	// Start server in a goroutine
	go func() {
		logger.Info(fmt.Sprintf("Server starting on port %s", cfg.ServerConfig.Port))
		if err := srv.ListenAndServe(); err != nil && err != http.ErrServerClosed {
			logger.Error(fmt.Sprintf("Server failed to start: %v", err))
			os.Exit(1)
		}
	}()

	// Wait for interrupt signal for graceful shutdown
	quit := make(chan os.Signal, 1)
	signal.Notify(quit, syscall.SIGINT, syscall.SIGTERM)
	<-quit

	logger.Info("Shutting down server...")

	// Graceful shutdown with 10 second timeout
	ctx, cancel := context.WithTimeout(context.Background(), 10*time.Second)
	defer cancel()

	if err := srv.Shutdown(ctx); err != nil {
		logger.Error(fmt.Sprintf("Server forced to shutdown: %v", err))
	}

	logger.Info("Server stopped gracefully")
}

func initDB(cfg config.DatabaseConfig) (*gorm.DB, error) {
	var db *gorm.DB
	var err error
	maxAttempts := 10

	for attempt := 1; attempt <= maxAttempts; attempt++ {
		db, err = gorm.Open(postgres.Open(cfg.DSN), &gorm.Config{})
		if err == nil {
			sqlDB, pingErr := db.DB()
			if pingErr == nil {
				pingErr = sqlDB.Ping()
			}
			if pingErr == nil {
				// Configure connection pool
				sqlDB.SetMaxOpenConns(cfg.MaxOpenConns)
				sqlDB.SetMaxIdleConns(cfg.MaxIdleConns)
				sqlDB.SetConnMaxLifetime(cfg.ConnMaxLifetime)
				break
			}
			err = pingErr
		}
		logger.Info(fmt.Sprintf("Database connection attempt %d/%d failed: %v", attempt, maxAttempts, err))
		time.Sleep(2 * time.Second)
	}

	if err != nil {
		return nil, fmt.Errorf("failed to connect to database after %d attempts: %v", maxAttempts, err)
	}

	// Auto migrate schema
	if err := db.AutoMigrate(&models.User{}, &models.SpreadsheetData{}, &models.SheetFile{}, &models.SheetFileShare{}); err != nil {
		return nil, fmt.Errorf("failed to migrate database: %v", err)
	}

	logger.Info("Database migration completed successfully")
	return db, nil
}

// readCSVRows loads a CSV file from disk into [][]string while preserving empty fields.
func readCSVRows(path string) ([][]string, error) {
	f, err := os.Open(path)
	if err != nil {
		return nil, err
	}
	defer f.Close()

	reader := csv.NewReader(f)
	reader.FieldsPerRecord = -1 // allow variable columns per row
	var rows [][]string
	for {
		record, err := reader.Read()
		if err == io.EOF {
			break
		}
		if err != nil {
			return nil, err
		}
		rows = append(rows, record)
	}
	return rows, nil
}
