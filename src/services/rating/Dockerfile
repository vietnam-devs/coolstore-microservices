FROM node:alpine

RUN mkdir -p /usr/src/services/rating
WORKDIR /usr/src/services/rating

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}
EXPOSE 5007
ENV NODE_ENV production

COPY ./src/services/rating/package.json /usr/src/services/rating/
RUN yarn --network-timeout 100000 install

COPY ./src/services/rating/ /usr/src/services/rating/

CMD ["yarn","start"]
