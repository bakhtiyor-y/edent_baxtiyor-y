#STAGE 1
FROM node:16.20.0-alpine AS build
WORKDIR /usr/src/app

COPY package.json package-lock.json ./
COPY . .
RUN npm install
RUN npm run build

#STAGE 2
FROM nginx:alpine
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=build /usr/src/app/dist/edent /usr/share/nginx/html