FROM node:alpine AS caff-store-build
WORKDIR /app
COPY . .
RUN npm ci && npm run build

FROM nginx:alpine
COPY --from=caff-store-build /app/dist/caff-store /usr/share/nginx/html
EXPOSE 80