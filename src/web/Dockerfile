FROM node:alpine

RUN mkdir -p /usr/src/web
WORKDIR /usr/src/web

COPY ./src/web/package.json /usr/src/web/package.json

# RUN yarn --network-timeout 100000 install
RUN npm install

COPY ./src/web/ /usr/src/web/
RUN npm run build

EXPOSE 8080

CMD ["npm", "start"]
