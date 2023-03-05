FROM node:18-alpine as build
WORKDIR /usr/app
COPY frontend/agile4all/ /usr/app
RUN npm ci
RUN npm run build