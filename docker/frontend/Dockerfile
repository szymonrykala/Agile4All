FROM node:18-bullseye-slim
WORKDIR /usr/app
COPY frontend/agile4all/ /usr/app
RUN npm ci
RUN npm run build

RUN npm i -g serve

ENTRYPOINT serve -s build