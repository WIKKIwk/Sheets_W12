package config

// Logging configuration for Go backend
// This is a placeholder/example file

import (
	"log"
	"os"
)

// LogLevel represents logging verbosity
type LogLevel string

const (
	DEBUG LogLevel = "debug"
	INFO  LogLevel = "info"
	WARN  LogLevel = "warn"
	ERROR LogLevel = "error"
)

// Logger interface for application logging
type Logger interface {
	Debug(msg string)
	Info(msg string)
	Warn(msg string)
	Error(msg string)
}

// SimpleLogger basic logger implementation
type SimpleLogger struct {
	level LogLevel
}

// NewLogger creates a new logger instance
func NewLogger(level LogLevel) *SimpleLogger {
	return &SimpleLogger{level: level}
}

// Debug logs debug messages
func (l *SimpleLogger) Debug(msg string) {
	if l.level == DEBUG {
		log.Println("[DEBUG]", msg)
	}
}

// Info logs info messages
func (l *SimpleLogger) Info(msg string) {
	log.Println("[INFO]", msg)
}

// Warn logs warning messages
func (l *SimpleLogger) Warn(msg string) {
	log.Println("[WARN]", msg)
}

// Error logs error messages
func (l *SimpleLogger) Error(msg string) {
	log.SetOutput(os.Stderr)
	log.Println("[ERROR]", msg)
	log.SetOutput(os.Stdout)
}
