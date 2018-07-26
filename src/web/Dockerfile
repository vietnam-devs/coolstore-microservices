FROM node:alpine

ENV WEB_HOST_ALIAS webhostalias
ENV ID_HOST_ALIAS idhostalias
ENV API_HOST_ALIAS apihostalias

RUN mkdir -p /usr/src/web
WORKDIR /usr/src/web

COPY ./src/web/package.json /usr/src/web/

RUN yarn --network-timeout 100000 install

COPY ./src/web/ /usr/src/web/

EXPOSE 8080

RUN chmod +x script.sh

ENTRYPOINT ["./script.sh"]
