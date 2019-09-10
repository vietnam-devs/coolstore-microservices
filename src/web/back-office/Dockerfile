FROM node:alpine

RUN mkdir -p /usr/src/backoffice
WORKDIR /usr/src/backoffice

COPY ./src/backoffice/package.json /usr/src/backoffice/

RUN yarn --network-timeout 100000 install

COPY ./src/backoffice/ /usr/src/backoffice/

RUN yarn run build

EXPOSE 3000

ENTRYPOINT ["yarn", "start"]
