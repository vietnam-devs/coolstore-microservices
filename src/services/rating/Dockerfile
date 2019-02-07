FROM node:alpine
RUN npm install -g typescript

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}

WORKDIR /app

COPY ./src/services/rating/package*.json ./

RUN yarn install

COPY ./src/services/rating .

EXPOSE 5007
ENV NODE_ENV production

CMD ["yarn", "start"]
