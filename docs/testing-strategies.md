# Testing Strategies

## Test Pyramid

```
        /\
       /E2E\
      /------\
     /Integr.\
    /----------\
   /   Unit     \
  /--------------\
```

## Unit Tests

Test individual functions in isolation.

**Example:**
```go
func TestCalculateSum(t *testing.T) {
    result := CalculateSum(2, 3)
    if result != 5 {
        t.Errorf("Expected 5, got %d", result)
    }
}
```

## Integration Tests

Test API endpoints with real dependencies.

```go
func TestUserRegistration(t *testing.T) {
    // Setup test database
    db := setupTestDB()
    defer db.Close()
    
    // Test endpoint
    resp := testRequest("POST", "/register", userData)
    assert.Equal(t, 201, resp.StatusCode)
}
```

## E2E Tests

Test complete user workflows.

```javascript
test('user can create spreadsheet', async () => {
  await page.goto('http://localhost:8001');
  await page.click('#login');
  await page.fill('#email', 'test@example.com');
  await page.fill('#password', 'password');
  await page.click('#submit');
  
  await page.click('#new-file');
  expect(await page.title()).toBe('Untitled - W12C Sheets');
});
```

## Test Data

### Fixtures
```go
var testUsers = []User{
    {Email: "test1@example.com", Password: "pass123"},
    {Email: "test2@example.com", Password: "pass456"},
}
```

### Factories
```go
func CreateTestUser(email string) *User {
    return &User{
        Email:    email,
        Password: HashPassword("password"),
        FullName: "Test User",
    }
}
```

## Mocking

```go
type MockDatabase struct{}

func (m *MockDatabase) GetUser(id int) (*User, error) {
    return &User{ID: id, Email: "mock@example.com"}, nil
}
```

## Coverage

```bash
# Go tests with coverage
go test ./... -coverprofile=coverage.out
go tool cover -html=coverage.out

# JavaScript coverage
npm test -- --coverage
```

## CI/CD Integration

Run tests automatically on:
- Every commit
- Pull requests
- Before deployment

```yaml
# .github/workflows/test.yml
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - run: go test ./...
```
