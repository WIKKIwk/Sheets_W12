package utils

import (
	"bytes"
	"errors"
	"io"
	"mime/multipart"
	"net/http"
	"strings"
	"unicode"
)

// ValidateFile checks the file magic number to ensure it's a valid Excel or CSV file
func ValidateFile(file multipart.File) error {
	// Read the first 512 bytes to detect content type
	buffer := make([]byte, 512)
	_, err := file.Read(buffer)
	if err != nil && err != io.EOF {
		return err
	}

	// Reset file pointer
	file.Seek(0, 0)

	contentType := http.DetectContentType(buffer)

	// Magic numbers/signatures
	// ZIP (xlsx is a zip): PK..
	isZip := bytes.HasPrefix(buffer, []byte{0x50, 0x4B, 0x03, 0x04})

	// OLE2 (xls): D0 CF 11 E0
	isOle := bytes.HasPrefix(buffer, []byte{0xD0, 0xCF, 0x11, 0xE0})

	// CSV is harder as it's plain text, but we can check if it looks like text
	isText := contentType == "text/plain" ||
		contentType == "text/csv" ||
		contentType == "application/csv" ||
		contentType == "application/vnd.ms-excel" ||
		contentType == "application/octet-stream" ||
		strings.HasPrefix(contentType, "text/") ||
		isLikelyText(buffer)

	if isZip || isOle || isText {
		return nil
	}

	return errors.New("invalid file signature: not an Excel or CSV file")
}

// isLikelyText checks if the buffer is predominantly printable/textual data.
func isLikelyText(buf []byte) bool {
	if len(buf) == 0 {
		return false
	}
	printable := 0
	for _, b := range buf {
		if b == 0 {
			// NUL bytes usually indicate binary
			return false
		}
		if (b >= 32 && b <= 126) || b == '\n' || b == '\r' || b == '\t' {
			printable++
		}
	}
	ratio := float64(printable) / float64(len(buf))
	return ratio >= 0.75
}

// ValidatePassword checks password strength requirements
// Minimum 12 characters with at least one uppercase, one lowercase, one digit, and one special character
func ValidatePassword(password string) error {
	if len(password) < 12 {
		return errors.New("password must be at least 12 characters long")
	}

	var (
		hasUpper   bool
		hasLower   bool
		hasDigit   bool
		hasSpecial bool
	)

	for _, char := range password {
		switch {
		case unicode.IsUpper(char):
			hasUpper = true
		case unicode.IsLower(char):
			hasLower = true
		case unicode.IsDigit(char):
			hasDigit = true
		case unicode.IsPunct(char) || unicode.IsSymbol(char):
			hasSpecial = true
		}
	}

	if !hasUpper {
		return errors.New("password must contain at least one uppercase letter")
	}
	if !hasLower {
		return errors.New("password must contain at least one lowercase letter")
	}
	if !hasDigit {
		return errors.New("password must contain at least one digit")
	}
	if !hasSpecial {
		return errors.New("password must contain at least one special character")
	}

	return nil
}
