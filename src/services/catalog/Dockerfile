FROM node:alpine

RUN apk add --no-cache curl
RUN curl -sL https://unpkg.com/@pnpm/self-installer | node

RUN mkdir -p /usr/src/services/catalog
WORKDIR /usr/src/services/catalog

COPY ./src/services/catalog/package.json /usr/src/services/catalog/
RUN pnpm install
ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}
COPY ./src/services/catalog/ /usr/src/services/catalog/

EXPOSE 3000

CMD ["pnpm", "start"]
