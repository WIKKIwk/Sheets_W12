# Release Process

## Steps

1. Update CHANGELOG.md
2. Bump version in files
3. Create git tag
4. Push to GitHub
5. Build Docker images
6. Deploy to production
7. Monitor for issues

## Versioning

Follow semantic versioning:
- MAJOR: Breaking changes
- MINOR: New features
- PATCH: Bug fixes

## Example
```bash
git tag v1.1.0
git push origin v1.1.0
```
