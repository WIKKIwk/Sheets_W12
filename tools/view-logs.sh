#!/bin/bash
SERVICE=${1:-backend-go}
docker compose logs -f --tail=100 $SERVICE
