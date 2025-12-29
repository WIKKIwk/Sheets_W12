# Utility Scripts

Collection of helper scripts for W12C Sheets development and operations.

## Scripts

### Development

**`run-tests.sh`**
Run all test suites (Go, frontend)
```bash
./scripts/run-tests.sh
```

**`monitor.sh`**
Performance monitoring dashboard
```bash
./scripts/monitor.sh
```

**`validate-config.sh`**
Validate environment configuration
```bash
./scripts/validate-config.sh
```

### Database

**`db-console.sh`**
Open PostgreSQL console
```bash
./scripts/db-console.sh
```

**`auto-backup.sh`**
Create database backup
```bash
./scripts/auto-backup.sh
```

### Docker

**`cleanup-docker.sh`**
Clean up Docker resources
```bash
./scripts/cleanup-docker.sh
```

**`view-logs.sh`**
View service logs
```bash
./scripts/view-logs.sh backend-go
```

### Deployment

**`switch-env.sh`**
Switch environment configuration
```bash
./scripts/switch-env.sh production
```

**`release.sh`**
Create new release
```bash
./scripts/release.sh v1.2.0
```

**`update.sh`**
Update running deployment
```bash
./scripts/update.sh
```

## Automated Tasks

### Cron Jobs

Add to crontab for automated backups:
```bash
crontab -e

# Daily backup at 2 AM
0 2 * * * /path/to/scripts/auto-backup.sh
```

## Usage Tips

- Make scripts executable: `chmod +x scripts/*.sh`
- Add scripts directory to PATH for convenience
- Check script output for errors
- Use scripts in CI/CD pipelines

## Creating New Scripts

When adding new scripts:
1. Add execute permission
2. Include usage comments
3. Handle errors properly
4. Update this README
