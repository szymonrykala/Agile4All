FROM ghcr.io/blayeer/agile-project/frontend:latest as frontend

FROM nginx:alpine
EXPOSE 80
COPY ./docker/nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=frontend /usr/app/build /usr/share/nginx/html
