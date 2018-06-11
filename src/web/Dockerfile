FROM node:10.0.0-slim

RUN mkdir -p /usr/src/web
WORKDIR /usr/src/web

COPY ./package.json /usr/src/web/package.json
RUN yarn install

COPY ./ /usr/src/web/
RUN yarn build

EXPOSE 3000

CMD ["yarn","start"]
