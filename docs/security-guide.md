# Security Best Practices for W12C Sheets

## Overview

Security is paramount in W12C Sheets. This guide covers essential security practices for development, deployment, and operations.

## Authentication & Authorization

### 1. JWT Token Security

**Strong Secret Keys:**

```bash
# Generate strong JWT secret (minimum 64 characters)
openssl rand -base64 64

# Store in environment variable, never in code
JWT_SECRET=your_generated_secret_here
```

**Token Configuration:**

```go
// Set appropriate expiration
claims["exp"] = time.Now().Add(24 * time.Hour).Unix()

// Include minimal claims
claims["user_id"] = user.ID
claims["email"] = user.Email
claims["role"] = user.Role
```

**Token Validation:**

```go
func ValidateToken(tokenString string) (*Claims, error) {
  token, err := jwt.ParseWithClaims(tokenString, &Claims{}, func(t *jwt.Token) (interface{}, error) {
    // Verify signing method
    if _, ok := t.Method.(*jwt.SigningMethodHMAC); !ok {
      return nil, fmt.Errorf("unexpected signing method")
    }
    return []byte(os.Getenv("JWT_SECRET")), nil
  })
  
  if err != nil || !token.Valid {
    return nil, err
  }
  
  return token.Claims.(*Claims), nil
}
```

### 2. Password Security

**Hashing:**

```go
import "golang.org/x/crypto/bcrypt"

// Hash password with appropriate cost
func HashPassword(password string) (string, error) {
  // Cost 12 = ~250ms on modern hardware
  bytes, err := bcrypt.GenerateFromPassword([]byte(password), 12)
  return string(bytes), err
}

// Verify password
func CheckPassword(password, hash string) bool {
  err := bcrypt.CompareHashAndPassword([]byte(hash), []byte(password))
  return err == nil
}
```

**Password Requirements:**

- Minimum 8 characters
- Must include: uppercase, lowercase, number, special character
- Prevent common passwords (use dictionary check)
- Implement rate limiting on login attempts

### 3. Role-Based Access Control (RBAC)

**Permission Levels:**

```go
type Role string

const (
  RoleOwner  Role = "owner"   // Full access
  RoleEditor Role = "editor"  // Can edit, no delete
  RoleViewer Role = "viewer"  // Read-only
)

func CheckPermission(user *User, file *File, action string) bool {
  share, _ := GetShare(file.ID, user.ID)
  
  switch action {
  case "read":
    return share != nil  // Any role can read
  case "write":
    return share.Role == RoleOwner || share.Role == RoleEditor
  case "delete":
    return share.Role == RoleOwner
  case "share":
    return share.Role == RoleOwner
  default:
    return false
  }
}
```

## Input Validation & Sanitization

### 1. Formula Validation

**Prevent Injection:**

```go
func ValidateFormula(formula string) error {
  if !strings.HasPrefix(formula, "=") {
    return errors.New("formula must start with =")
  }
  
  // Max formula length
  if len(formula) > 8192 {
    return errors.New("formula too long")
  }
  
  // Disallow dangerous functions (if any)
  dangerous := []string{"SYSTEM", "EXEC", "SHELL"}
  upper := strings.ToUpper(formula)
  for _, d := range dangerous {
    if strings.Contains(upper, d) {
      return errors.New("dangerous function detected")
    }
  }
  
  return nil
}
```

### 2.Cell Value Validation

**Size Limits:**

```go
const (
  MaxCellValueLength = 32767  // Excel limit
  MaxFormulaLength   = 8192
)

func ValidateCellValue(value string) error {
  if len(value) > MaxCellValueLength {
    return errors.New("value exceeds maximum length")
  }
  return nil
}
```

**Sanitize HTML:**

```go
import "html"

func SanitizeCellValue(value string) string {
  // Escape HTML to prevent XSS
  return html.EscapeString(value)
}
```

### 3. API Input Validation

**Request Validation:**

```go
type CreateFileRequest struct {
  Name  string `json:"name" binding:"required,min=1,max=255"`
  State string `json:"state" binding:"max=1048576"` // 1MB max
}

func CreateFile(c *gin.Context) {
  var req CreateFileRequest
  
  if err := c.ShouldBindJSON(&req); err != nil {
    c.JSON(400, gin.H{"error": "Invalid input", "details": err.Error()})
    return
  }
  
  // Additional validation
  if strings.TrimSpace(req.Name) == "" {
    c.JSON(400, gin.H{"error": "Name cannot be empty"})
    return
  }
  
  // Process request...
}
```

## Rate Limiting

### 1. API Rate Limiting

**Per-User Limits:**

```go
import "golang.org/x/time/rate"

var limiters = make(map[string]*rate.Limiter)
var mu sync.Mutex

func GetLimiter(userID string) *rate.Limiter {
  mu.Lock()
  defer mu.Unlock()
  
  if limiter, exists := limiters[userID]; exists {
    return limiter
  }
  
  // 100 requests per second, burst of 10
  limiter := rate.NewLimiter(100, 10)
  limiters[userID] = limiter
  return limiter
}

func RateLimitMiddleware() gin.HandlerFunc {
  return func(c *gin.Context) {
    userID := c.GetString("user_id")
    limiter := GetLimiter(userID)
    
    if !limiter.Allow() {
      c.JSON(429, gin.H{"error": "Too many requests"})
      c.Abort()
      return
    }
    
    c.Next()
  }
}
```

### 2. Login Attempt Limiting

**Prevent Brute Force:**

```go
type LoginAttempts struct {
  Count     int
  BlockedUntil time.Time
}

var attempts = make(map[string]*LoginAttempts)

func CheckLoginAttempts(email string) error {
  if attempt, exists := attempts[email]; exists {
    if time.Now().Before(attempt.BlockedUntil) {
      return fmt.Errorf("account locked until %v", attempt.BlockedUntil)
    }
    
    if attempt.Count >= 5 {
      // Lock for 15 minutes after 5 failed attempts
      attempt.BlockedUntil = time.Now().Add(15 * time.Minute)
      return fmt.Errorf("too many failed attempts, locked for 15 minutes")
    }
  }
  return nil
}

func RecordFailedLogin(email string) {
  if _, exists := attempts[email]; !exists {
    attempts[email] = &LoginAttempts{}
  }
  attempts[email].Count++
}

func ClearLoginAttempts(email string) {
  delete(attempts, email)
}
```

## CORS Configuration

**Strict CORS Policy:**

```go
import "github.com/gin-contrib/cors"

func SetupCORS() gin.HandlerFunc {
  config := cors.Config{
    // Specific origins only
    AllowOrigins: strings.Split(os.Getenv("ALLOWED_ORIGINS"), ","),
    
    // Allowed methods
    AllowMethods: []string{"GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS"},
    
    // Allowed headers
    AllowHeaders: []string{"Origin", "Content-Type", "Authorization"},
    
    // Expose headers
    ExposeHeaders: []string{"Content-Length"},
    
    // Credentials
    AllowCredentials: true,
    
    // Preflight cache
    MaxAge: 12 * time.Hour,
  }
  
  return cors.New(config)
}
```

**Production Configuration:**

```bash
# .env.production
ALLOWED_ORIGINS=https://yourdomain.com,https://app.yourdomain.com
```

## Database Security

### 1. SQL Injection Prevention

**Use Parameterized Queries:**

```go
// Good: Parameterized query
db.Where("email = ?", email).First(&user)

// Bad: String concatenation
db.Where(fmt.Sprintf("email = '%s'", email)).First(&user) // VULNERABLE!
```

**GORM Protects by Default:**

```go
// GORM automatically escapes parameters
db.Where("name LIKE ?", "%"+search+"%").Find(&files)
```

### 2. Database Access Control

**Principle of Least Privilege:**

```sql
-- Create application user with limited permissions
CREATE USER w12c_app WITH PASSWORD 'strong_password';

-- Grant only necessary permissions
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO w12c_app;

-- Revoke dangerous permissions
REVOKE CREATE, DROP ON DATABASE w12c_db FROM w12c_app;
```

### 3. Encryption at Rest

**PostgreSQL:**

```bash
# Enable transparent data encryption
initdb --data-encryption

# Or use encrypted filesystem
# LUKS, dm-crypt, etc.
```

## Network Security

### 1. HTTPS/TLS

**Force HTTPS:**

```nginx
server {
  listen 80;
  server_name yourdomain.com;
  
  # Redirect to HTTPS
  return 301 https://$server_name$request_uri;
}

server {
  listen 443 ssl http2;
  server_name yourdomain.com;
  
  # SSL certificates
  ssl_certificate /etc/letsencrypt/live/yourdomain.com/fullchain.pem;
  ssl_certificate_key /etc/letsencrypt/live/yourdomain.com/privkey.pem;
  
  # Strong SSL configuration
  ssl_protocols TLSv1.2 TLSv1.3;
  ssl_ciphers 'ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256';
  ssl_prefer_server_ciphers on;
  
  # HSTS
  add_header Strict-Transport-Security "max-age=31536000; includeSubDomains" always;
}
```

### 2. Security Headers

**Nginx Configuration:**

```nginx
# Prevent clickjacking
add_header X-Frame-Options "SAMEORIGIN" always;

# XSS protection
add_header X-Content-Type-Options "nosniff" always;
add_header X-XSS-Protection "1; mode=block" always;

# Content Security Policy
add_header Content-Security-Policy "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline';" always;

# Referrer policy
add_header Referrer-Policy "strict-origin-when-cross-origin" always;
```

## API Security

### 1. API Key Management

**Secure Storage:**

```go
// Store API keys hashed
func StoreGeminiKey(userID string, apiKey string) error {
  // Hash the key
  hashedKey, _ := bcrypt.GenerateFromPassword([]byte(apiKey), 12)
  
  // Store only hash
  return db.Model(&User{}).Where("id = ?", userID).
    Update("gemini_key_hash", hashedKey).Error
}
```

**Encryption for Sensitive Data:**

```go
import "crypto/aes"
import "crypto/cipher"

func EncryptAPIKey(plaintext string) (string, error) {
  key := []byte(os.Getenv("ENCRYPTION_KEY")) // 32 bytes for AES-256
  
  block, _ := aes.NewCipher(key)
  gcm, _ := cipher.NewGCM(block)
  nonce := make([]byte, gcm.NonceSize())
  
  ciphertext := gcm.Seal(nonce, nonce, []byte(plaintext), nil)
  return base64.StdEncoding.EncodeToString(ciphertext), nil
}
```

### 2. Request Authentication

**Verify All Requests:"** ```go
func AuthMiddleware() gin.HandlerFunc {
  return func(c *gin.Context) {
    authHeader := c.GetHeader("Authorization")
    if authHeader == "" {
      c.JSON(401, gin.H{"error": "No authorization header"})
      c.Abort()
      return
    }

    // Extract token
    parts := strings.Split(authHeader, " ")
    if len(parts) != 2 || parts[0] != "Bearer" {
      c.JSON(401, gin.H{"error": "Invalid authorization format"})
      c.Abort()
      return
    }
    
    // Validate token
    claims, err := ValidateToken(parts[1])
    if err != nil {
      c.JSON(401, gin.H{"error": "Invalid token"})
      c.Abort()
      return
    }
    
    // Store user info in context
    c.Set("user_id", claims.UserID)
    c.Set("email", claims.Email)
    c.Next()
  }
}

```

## WebSocket Security

### 1. Connection Authentication

**Token-based Auth:**
```elixir
def join("room:" <> file_id, %{"token" => token}, socket) do
  case verify_token(token) do
    {:ok, user_id} ->
      # Verify user has access to file
      case has_access?(user_id, file_id) do
        true ->
          {:ok, assign(socket, :user_id, user_id)}
        false ->
          {:error, %{reason: "unauthorized"}}
      end
    {:error, _} ->
      {:error, %{reason: "invalid_token"}}
  end
end
```

### 2. Message Validation

**Validate All Messages:**

```elixir
def handle_in("cell_update", %{"cell_id" => cell_id, "value" => value}, socket) do
  # Validate cell_id format
  unless valid_cell_id?(cell_id) do
    {:reply, {:error, %{reason: "invalid_cell_id"}}, socket}
  end
  
  # Validate value size
  if String.length(value) > 32767 do
    {:reply, {:error, %{reason: "value_too_large"}}, socket}
  end
  
  # Process update...
  {:noreply, socket}
end
```

## Monitoring & Auditing

### 1. Security Logging

**Log Security Events:**

```go
func LogSecurityEvent(eventType, userID, details string) {
  log.WithFields(log.Fields{
    "event_type": eventType,
    "user_id":    userID,
    "details":    details,
    "timestamp":  time.Now(),
    "ip":         getClientIP(),
  }).Warn("Security event")
}

// Usage
LogSecurityEvent("failed_login", email, "Invalid password")
LogSecurityEvent("unauthorized_access", userID, fmt.Sprintf("Attempted to access file %s", fileID))
```

### 2. Audit Trail

**Track Important Actions:**

```go
type AuditLog struct {
  ID        uint
  UserID    string
  Action    string
  Resource  string
  Details   string
  IP        string
  CreatedAt time.Time
}

func CreateAuditLog(userID, action, resource, details, ip string) {
  log := AuditLog{
    UserID:   userID,
    Action:   action,
    Resource: resource,
    Details:  details,
    IP:       ip,
  }
  db.Create(&log)
}
```

## Security Checklist

### Development

- [ ] Use parameterized queries (no string concatenation)
- [ ] Hash passwords with bcrypt (cost >= 12)
- [ ] Validate all user input
- [ ] Sanitize output to prevent XSS
- [ ] Use HTTPS for all connections
- [ ] Implement proper error handling (no stack traces to users)

### Deployment

- [ ] Generate strong secrets (64+ characters)
- [ ] Configure CORS properly
- [ ] Enable rate limiting
- [ ] Set up security headers
- [ ] Use HTTPS/TLS
- [ ] Implement audit logging
- [ ] Regular security updates

### Operations

- [ ] Monitor security logs
- [ ] Regular security audits
- [ ] Update dependencies
- [ ] Backup encryption keys
- [ ] Incident response plan
- [ ] Regular penetration testing

## Incident Response

### Steps for Security Incident

1. **Detection**: Monitor logs for suspicious activity
2. **Containment**: Block affected users/IPs
3. **Investigation**: Analyze logs and audit trail
4. **Remediation**: Fix vulnerability
5. **Recovery**: Restore service
6. **Review**: Post-mortem and improvements

## Resources

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)
- [PostgreSQL Security](https://www.postgresql.org/docs/current/security.html)
- [Go Security](https://go.dev/doc/security/)
