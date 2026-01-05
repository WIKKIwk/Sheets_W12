# Deployment Checklist

Use this checklist to ensure a smooth deployment to production.

## Pre-Deployment

### System Requirements

- [ ] Server specs: 2+ vCPU, 4GB+ RAM, 40GB+ SSD
- [ ] Ubuntu 22.04 LTS or similar
- [ ] Docker 20.10+ installed
- [ ] Docker Compose v2.0+ installed
- [ ] Domain name configured
- [ ] SSL certificate ready (Let's Encrypt recommended)

### Environment Configuration

- [ ] Copy `.env.production.example` to `.env.production`
- [ ] Generate strong DB_PASSWORD (48+ chars): `openssl rand -base64 48`
- [ ] Generate strong JWT_SECRET (64+ chars): `openssl rand -base64 64`
- [ ] Generate INTERNAL_API_SECRET: `openssl rand -base64 32`
- [ ] Generate ELIXIR_SECRET_KEY_BASE: `openssl rand -base64 64`
- [ ] Generate REDIS_PASSWORD: `openssl rand -base64 32`
- [ ] Set ALLOWED_ORIGINS to production domains
- [ ] Configure SMTP settings (if using email)
- [ ] Review all environment variables

### Security

- [ ] All secrets are strong and random
- [ ] No default passwords in use
- [ ] CORS origins restricted to production domains
- [ ] Rate limiting configured
- [ ] Firewall rules configured (ports 22, 80, 443)
- [ ] SSH key-based authentication enabled
- [ ] Root login disabled

### DNS Configuration

- [ ] A record: @ → server IP
- [ ] A record: www → server IP
- [ ] A record: api → server IP
- [ ] A record: realtime → server IP
- [ ] DNS propagation complete (use `dig yourdomain.com`)

## Deployment Steps

### 1. Server Setup

- [ ] Update system: `sudo apt update && sudo apt upgrade -y`
- [ ] Install Docker: `sudo apt install docker.io docker-compose-v2`
- [ ] Enable Docker: `sudo systemctl enable docker`
- [ ] Add user to docker group: `sudo usermod -aG docker $USER`
- [ ] Install certbot: `sudo apt install certbot python3-certbot-nginx`
- [ ] Configure firewall: See PRODUCTION_DEPLOYMENT.md

### 2. SSL Certificate

- [ ] Stop any services on port 80/443
- [ ] Request certificate: `sudo certbot certonly --standalone -d yourdomain.com`
- [ ] Copy certificates to `nginx/ssl/`
- [ ] Set correct permissions: `sudo chmod 644 nginx/ssl/*.pem`
- [ ] Test renewal: `sudo certbot renew --dry-run`

### 3. Application Deployment

- [ ] Clone repository to `/opt/w12c`
- [ ] Navigate to project directory
- [ ] Copy and configure `.env.production`
- [ ] Build images: `docker compose -f docker-compose.prod.yml build --no-cache`
- [ ] Start services: `docker compose -f docker-compose.prod.yml up -d`
- [ ] Wait for services to be healthy

### 4. Verification

- [ ] Check service status: `docker compose -f docker-compose.prod.yml ps`
- [ ] Check logs for errors: `docker compose -f docker-compose.prod.yml logs`
- [ ] Test health endpoint: `curl https://api.yourdomain.com/health`
- [ ] Test frontend: `curl https://yourdomain.com`
- [ ] Verify SSL: Check browser padlock icon
- [ ] Test user registration
- [ ] Test user login
- [ ] Create test spreadsheet
- [ ] Test real-time updates (multiple browsers)
- [ ] Test AI features (if API key provided)

## Post-Deployment

### Monitoring

- [ ] Set up log rotation
- [ ] Configure automated backups (cron job)
- [ ] Set up monitoring/alerting (optional)
- [ ] Test backup restoration process
- [ ] Document backup location and process

### Performance

- [ ] Run performance tests
- [ ] Check database query performance
- [ ] Monitor resource usage (CPU, RAM, disk)
- [ ] Optimize if needed

### Security

- [ ] Run security audit
- [ ] Test rate limiting
- [ ] Verify CORS configuration
- [ ] Check for exposed secrets
- [ ] Review firewall rules
- [ ] Set up fail2ban (optional)

### Documentation

- [ ] Document deployment process
- [ ] Document any custom configurations
- [ ] Share credentials with team (securely)
- [ ] Update README with production URLs

## Ongoing Maintenance

### Daily

- [ ] Check service health
- [ ] Monitor error logs
- [ ] Check disk space

### Weekly

- [ ] Review performance metrics
- [ ] Check backup integrity
- [ ] Update dependencies (if needed)

### Monthly

- [ ] Security updates
- [ ] Certificate renewal check
- [ ] Performance review
- [ ] Cleanup old backups

## Rollback Plan

If deployment fails:

1. **Stop new services:**

   ```bash
   docker compose -f docker-compose.prod.yml down
   ```

2. **Restore from backup:**

   ```bash
   ./scripts/restore.sh /backups/latest.sql.gz
   ```

3. **Start previous version:**

   ```bash
   # Checkout previous version
   git checkout previous-tag
   docker compose -f docker-compose.prod.yml up -d
   ```

4. **Verify restoration:**
   - Test health endpoint
   - Test user login
   - Verify data integrity

## Troubleshooting

### Services won't start

- Check logs: `docker compose logs`
- Verify environment variables
- Check port conflicts

### Database connection errors

- Verify DB_DSN in `.env.production`
- Check database is running
- Test connection manually

### SSL certificate issues

- Renew certificate: `sudo certbot renew`
- Check certificate expiration
- Verify nginx configuration

### High memory usage

- Check for memory leaks in logs
- Restart services
- Increase server RAM if needed

## Emergency Contacts

- System Admin: [contact]
- Database Admin: [contact]
- Security Team: [contact]

## Additional Resources

- [PRODUCTION_DEPLOYMENT.md](./PRODUCTION_DEPLOYMENT.md)
- [TROUBLESHOOTING.md](./docs/TROUBLESHOOTING.md)
- [SECURITY.md](./SECURITY.md)

---

**Deployment Date:** _____________  
**Deployed By:** _____________  
**Version:** _____________  
**Notes:** _____________
