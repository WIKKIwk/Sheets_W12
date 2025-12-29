# Configuration Files

This directory contains configuration files and examples for W12C Sheets.

## Files

### `logging.go`
Logger configuration for Go backend. Provides different log levels (debug, info, warn, error).

**Usage:**
```go
logger := config.NewLogger(config.INFO)
logger.Info("Server started")
logger.Error("Failed to connect")
```

## Environment Configuration

See root directory for environment templates:
- `.env.development` - Development settings
- `.env.staging` - Staging settings
- `.env.production.example` - Production template

## Best Practices

1. **Never commit secrets** - Use environment variables
2. **Use different configs** for different environments
3. **Rotate secrets regularly** - Especially JWT secrets
4. **Validate configuration** - Use `scripts/validate-config.sh`

## Adding New Configuration

When adding new config:
1. Add to appropriate environment template
2. Document in this README
3. Update validation script
4. Add example values
