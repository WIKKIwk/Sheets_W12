# W12C Sheets - Makefile
# Quick commands for common operations

.PHONY: help up down restart logs build test clean backup

help: ## Show this help
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'

up: ## Start all services
	docker compose up -d

down: ## Stop all services
	docker compose down

restart: ## Restart all services
	docker compose restart

logs: ## View logs (follow mode)
	docker compose logs -f

build: ## Rebuild all services
	docker compose build --no-cache

test: ## Run all tests
	./scripts/run-tests.sh

clean: ## Clean up Docker resources
	./scripts/cleanup-docker.sh

backup: ## Create database backup
	./scripts/auto-backup.sh

monitor: ## Monitor performance
	./scripts/monitor.sh

db: ## Open database console
	./scripts/db-console.sh

validate: ## Validate configuration
	./scripts/validate-config.sh

deploy-prod: ## Deploy to production
	docker compose -f docker-compose.prod.yml up -d --build
