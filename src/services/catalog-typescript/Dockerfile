FROM node:alpine

RUN mkdir -p /usr/src/services/catalog-typescript
WORKDIR /usr/src/services/catalog-typescript

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}
EXPOSE 5002
ENV NODE_ENV production

COPY ./src/services/catalog-typescript/ /usr/src/services/catalog-typescript/
RUN yarn install

ENTRYPOINT [ "yarn", "start" ]
