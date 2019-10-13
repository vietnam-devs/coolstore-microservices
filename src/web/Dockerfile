ARG NODE_IMAGE=node:10.16

FROM ${NODE_IMAGE} as node-build
WORKDIR /app

COPY src/web/package-lock.json .
COPY src/web/package.json .
RUN npm install

COPY src/web .
RUN npm run build

FROM node-build as publish

FROM nginx:perl as final
WORKDIR /app

RUN nginx -v
EXPOSE 31999

COPY --from=publish /app/nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=publish /app/nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=publish /app/build /app

CMD ["nginx", "-g", "daemon off;"]
