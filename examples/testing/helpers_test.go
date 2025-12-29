package testing

// Testing helper utilities
// Example test helpers for Go backend

import (
	"bytes"
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"testing"
)

// CreateTestRequest helper to create HTTP test requests
func CreateTestRequest(t *testing.T, method, url string, body interface{}) *http.Request {
	var buf bytes.Buffer
	if body != nil {
		if err := json.NewEncoder(&buf).Encode(body); err != nil {
			t.Fatal(err)
		}
	}
	
	req := httptest.NewRequest(method, url, &buf)
	req.Header.Set("Content-Type", "application/json")
	return req
}

// AssertStatusCode checks HTTP status code
func AssertStatusCode(t *testing.T, got, want int) {
	if got != want {
		t.Errorf("status code: got %d, want %d", got, want)
	}
}

// AssertJSONResponse decodes and validates JSON response
func AssertJSONResponse(t *testing.T, w *httptest.ResponseRecorder, target interface{}) {
	if err := json.NewDecoder(w.Body).Decode(target); err != nil {
		t.Fatalf("failed to decode response: %v", err)
	}
}

// Example test
// func TestUserRegistration(t *testing.T) {
//     req := CreateTestRequest(t, "POST", "/register", map[string]string{
//         "email": "test@example.com",
//         "password": "password123",
//     })
//     
//     w := httptest.NewRecorder()
//     handler.ServeHTTP(w, req)
//     
//     AssertStatusCode(t, w.Code, http.StatusCreated)
// }
