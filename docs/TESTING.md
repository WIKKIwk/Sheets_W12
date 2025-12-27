# Testing Guide

## Running Tests

### Backend Tests (Go)

```bash
cd backend-go
go test ./... -v
```

### Frontend Tests

```bash
cd shlyux
npm test
```

### Integration Tests

```bash
npm run test:integration
```

## Writing Tests

### Go Test Example

```go
func TestUserRegistration(t *testing.T) {
    // Test implementation
}
```

### React Test Example

```typescript
describe('Grid Component', () => {
  it('renders cells correctly', () => {
    // Test implementation
  });
});
```
