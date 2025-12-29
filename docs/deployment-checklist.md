# Production Deployment Checklist

## Pre-Deployment

- [ ] All tests passing locally
- [ ] Database migrations tested
- [ ] Environment variables configured
- [ ] SSL certificates ready
- [ ] Backup created
- [ ] Rollback plan prepared

## Security

- [ ] JWT secret is strong (64+ chars)
- [ ] Database password is secure
- [ ] CORS configured properly
- [ ] Rate limiting enabled
- [ ] Security headers configured
- [ ] Firewall rules set

## Configuration

- [ ] .env.production file ready
- [ ] Database connection tested
- [ ] Redis connection tested
- [ ] Email/SMTP configured
- [ ] Logging level set to 'production'
- [ ] Debug mode disabled

## Deployment

- [ ] Pull latest code
- [ ] Build Docker images
- [ ] Run database migrations
- [ ] Start services
- [ ] Verify health endpoints
- [ ] Test critical user flows

## Post-Deployment

- [ ] Monitor error logs
- [ ] Check response times
- [ ] Verify database connections
- [ ] Test WebSocket connections
- [ ] Monitor resource usage
- [ ] Notify team of deployment
