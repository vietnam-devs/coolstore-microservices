FROM node:alpine

RUN mkdir -p /usr/src/web
WORKDIR /usr/src/web

COPY ./src/web/package.json /usr/src/web/

RUN yarn --network-timeout 100000 install

COPY ./src/web/ /usr/src/web/

CMD ["sh", "-c", "sed -i -e 's/webhostalias/'$WEB_HOST_ALIAS'/g' -e 's/idhostalias/'$ID_HOST_ALIAS'/g' -e 's/apihostalias/'$API_HOST_ALIAS'/g' package.json"]
RUN yarn run build

EXPOSE 8080

RUN chmod +x script.sh

ENTRYPOINT ["./script.sh"]
