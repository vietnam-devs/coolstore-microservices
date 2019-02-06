FROM node:alpine
RUN npm install -g typescript

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}

WORKDIR /app

COPY ./src/services/catalog/package*.json ./

RUN yarn install

COPY ./src/services/catalog .

EXPOSE 5002
ENV NODE_ENV production

CMD ["yarn", "start"]
