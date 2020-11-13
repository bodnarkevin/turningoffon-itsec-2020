FROM node:alpine AS caff-store-build
WORKDIR /app
COPY . .
RUN npm ci
RUN npm run build -- --prod

FROM nginx:alpine
COPY --from=caff-store-build /app/dist/caff-store /usr/share/nginx/html
COPY --from=caff-store-build /app/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80