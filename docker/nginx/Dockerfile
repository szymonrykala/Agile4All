FROM blayer.mooo.com:5000/agile-project/frontend:latest as frontend

FROM nginx:alpine
EXPOSE 80
COPY ./docker/nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=frontend /usr/app/build /usr/share/nginx/html
