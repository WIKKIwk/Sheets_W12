# Build stage
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
ARG VITE_API_BASE=http://backend-go:8080
ENV VITE_API_BASE=${VITE_API_BASE}
ARG VITE_REALTIME_URL=ws://backend-elixir:4000/socket
ENV VITE_REALTIME_URL=${VITE_REALTIME_URL}
RUN npm run build

# Serve stage
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
# Simple SPA routing
COPY nginx.default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
