FROM node:alpine

RUN mkdir -p /usr/src/web
WORKDIR /usr/src/web

COPY ./src/web/package.json /usr/src/web/

RUN yarn --network-timeout 100000 install

COPY ./src/web/ /usr/src/web/

RUN yarn run build	

EXPOSE 8080

ENTRYPOINT ["yarn", "start"]
