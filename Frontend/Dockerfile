# ==========================================
# Stage 1: Build the React application
# ==========================================
FROM node:20-alpine AS build

# Set working directory
WORKDIR /app

# Copy package files first (better layer caching)
COPY package*.json ./

# Install dependencies (use npm ci if package-lock.json exists, otherwise npm install)
RUN if [ -f package-lock.json ]; then npm ci; else npm install; fi

# Copy the rest of the source code
COPY . .

# Build the production bundle (generates dist/index.html - single-file build)
RUN npm run build


# ==========================================
# Stage 2: Serve the app with Nginx
# ==========================================
FROM nginx:1.27-alpine AS runtime

# Remove default nginx config
RUN rm /etc/nginx/conf.d/default.conf

# Copy custom nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy built assets from the build stage
COPY --from=build /app/dist /usr/share/nginx/html

# Expose port 80 for the web server
EXPOSE 80

# Healthcheck to make sure nginx is responding
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:80/ || exit 1

# Run nginx in the foreground
CMD ["nginx", "-g", "daemon off;"]
