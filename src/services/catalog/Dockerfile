FROM node:alpine

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}
EXPOSE 5002
ENV NODE_ENV production

WORKDIR /app

COPY ./src/services/catalog/src .
RUN yarn install

ENTRYPOINT ["yarn", "start"]
