# Security Best Practices

## Authentication & Authorization

### JWT
- Use strong secrets (64+ characters)
- Set appropriate expiration times
- Rotate secrets regularly
- Store securely

### Passwords
- Minimum 8 characters
- Require complexity
- Use bcrypt for hashing
- Implement rate limiting on login

## Database Security

### Access Control
- Use least privilege principle
- Separate users for different environments
- Limit remote access
- Regular audits

### SQL Injection Prevention
- Always use parameterized queries
- Validate all inputs
- Use ORM properly

## API Security

### Rate Limiting
```go
// Limit requests per IP
limiter := rate.NewLimiter(100, 10)
```

### CORS
- Whitelist specific origins
- Don't use wildcard in production
- Validate all origins

### Headers
```
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
X-XSS-Protection: 1; mode=block
Strict-Transport-Security: max-age=31536000
```

## Production Checklist

- [ ] HTTPS everywhere
- [ ] Strong passwords
- [ ] JWT secrets rotated
- [ ] Database backups enabled
- [ ] Firewall configured
- [ ] Security headers set
- [ ] Dependencies updated
- [ ] Logs monitored
- [ ] Incident response plan ready

## Regular Maintenance

### Weekly
- Review access logs
- Check for failed login attempts
- Update dependencies

### Monthly
- Rotate secrets
- Review permissions
- Security audit
- Backup testing

### Quarterly
- Penetration testing
- Security training
- Incident response drill
