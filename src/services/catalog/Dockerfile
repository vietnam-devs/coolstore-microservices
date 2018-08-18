FROM node:alpine

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}
EXPOSE 5002
ENV NODE_ENV production

RUN mkdir -p /app/dist
WORKDIR /app
COPY ./src/services/catalog/ .
RUN npm install --silent -g rimraf cross-env npm-run-all webpack nodemon ts-node typescript webpack-cli
RUN yarn install
RUN yarn add webpack-shell-plugin --dev
RUN chmod +x ./script.sh
ENTRYPOINT ["./script.sh"]
