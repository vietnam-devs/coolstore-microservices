FROM node:alpine

RUN mkdir -p /app
WORKDIR /app

COPY src/services/open-api/package*.json ./

RUN yarn install

COPY src/services/open-api/ .

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}
EXPOSE 5010
ENV NODE_ENV production

ENTRYPOINT ["yarn", "start"]
