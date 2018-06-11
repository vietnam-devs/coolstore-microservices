FROM node:10.0.0-slim

RUN mkdir -p /usr/src/web
WORKDIR /usr/src/web

# COPY ./src/web/package.json /usr/src/web/package.json
# RUN yarn install

COPY ./src/web/ /usr/src/web/
RUN yarn build

EXPOSE 8080

CMD ["yarn","start"]
