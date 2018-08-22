FROM node:alpine

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}
EXPOSE 5002
ENV NODE_ENV production

RUN mkdir -p /app/dist
WORKDIR /app

COPY ./src/services/catalog/src src
COPY ./src/services/catalog/package.json package.json
COPY ./src/services/catalog/tsconfig.json tsconfig.json
COPY ./src/services/catalog/tsoa.json tsoa.json
COPY ./src/services/catalog/webpack.config.js webpack.config.js
RUN yarn install && yarn add rimraf cross-env npm-run-all webpack ts-node typescript webpack-cli webpack-shell-plugin ts-loader

ENTRYPOINT ["yarn", "start"]
