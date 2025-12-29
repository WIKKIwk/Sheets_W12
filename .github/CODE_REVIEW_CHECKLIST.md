# Code Review Checklist

## General

- [ ] Code follows style guidelines
- [ ] No commented-out code
- [ ] No debug statements
- [ ] Meaningful variable names
- [ ] Functions are focused and small
- [ ] No duplicate code

## Functionality

- [ ] Code works as intended
- [ ] Edge cases handled
- [ ] Error handling implemented
- [ ] Input validation present
- [ ] No hardcoded values

## Testing

- [ ] Tests added for new features
- [ ] Tests pass locally
- [ ] Coverage maintained or improved
- [ ] Integration tests if needed

## Security

- [ ] No secrets in code
- [ ] SQL injection prevented
- [ ] XSS protection
- [ ] CSRF protection
- [ ] Input sanitized
- [ ] Auth/authz checked

## Performance

- [ ] No N+1 queries
- [ ] Efficient algorithms
- [ ] Database indexes present
- [ ] No memory leaks
- [ ] Async operations where appropriate

## Documentation

- [ ] README updated if needed
- [ ] API docs updated
- [ ] Comments for complex logic
- [ ] CHANGELOG updated
- [ ] Migration guide if breaking change

## Before Merge

- [ ] CI/CD passing
- [ ] Conflicts resolved
- [ ] Branch up to date
- [ ] Reviewed by maintainer
- [ ] All comments addressed
