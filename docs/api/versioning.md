# API Versioning

## Version Strategy

W12C Sheets uses URL-based versioning for the API.

### Current Versions
- `v1` - Current stable version
- `v2` - Beta (future)

### URL Structure
```
/api/v1/files
/api/v2/files
```

## Version Support Policy

- **Stable versions** (v1): Supported for 24 months
- **Deprecated versions**: 12 months notice before removal
- **Beta versions**: No backward compatibility guarantee

## Breaking Changes

Changes that break compatibility:
- Removing fields from responses
- Changing field types
- Removing endpoints
- Changing authentication

Non-breaking changes:
- Adding new optional fields
- Adding new endpoints
- Adding new optional parameters

## Migration Guide

### From v1 to v2 (Future)

**Breaking Changes:**
- `file_id` renamed to `id`
- Date format changed to ISO 8601

**Example Migration:**
```javascript
// v1
const response = await fetch('/api/v1/files');
const files = response.json();
console.log(files[0].file_id);

// v2
const response = await fetch('/api/v2/files');
const files = response.json();
console.log(files[0].id);  // Changed
```

## Deprecation Notice

When deprecating an endpoint, we will:
1. Add `Deprecated` header to responses
2. Update documentation
3. Notify via changelog
4. Maintain for 12 months
5. Remove in next major version

### Example Deprecated Response
```
HTTP/1.1 200 OK
Deprecated: true
Sunset: Sun, 01 Jan 2026 00:00:00 GMT
```
