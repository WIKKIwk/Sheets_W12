# Code Examples

This directory contains example code and utilities for W12C Sheets.

## Directory Structure

### `/curl`
cURL examples for API testing
- `api-examples.sh` - Complete API workflow examples

### `/python`
Python examples and utilities
- `import_csv.py` - CSV import script
- `basic_usage.py` - Basic API usage

### `/javascript`
JavaScript/Node.js examples
- `batch-update.js` - Batch cell update

### `/formulas`
Formula examples and reference
- `common-formulas.md` - Commonly used formulas

### `/middleware`
Middleware examples (Go)
- `rate-limit.go` - Rate limiting middleware

### `/testing`
Testing utilities
- `helpers_test.go` - Test helper functions

### `/migrations`
Database migration examples
- `001_initial_schema.sql` - Initial schema

### `/utils`
Utility functions
- `errors.go` - Error handling
- `validator.go` - Request validation

## Usage

These examples are for reference and learning purposes. They demonstrate common patterns and best practices used in W12C Sheets.

### Running Examples

**cURL Examples:**
```bash
cd examples/curl
./api-examples.sh
```

**Python Examples:**
```bash
cd examples/python
python3 basic_usage.py
```

**JavaScript Examples:**
```bash
cd examples/javascript
node batch-update.js
```

## Testing

Example test files show how to write tests for W12C Sheets:

```bash
cd examples/testing
go test -v
```

## Contributing

When adding new examples:
1. Keep them simple and focused
2. Add comments explaining key concepts
3. Update this README
4. Ensure they work with current API
