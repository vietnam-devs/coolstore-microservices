FROM node:alpine

RUN mkdir -p /usr/src/services/catalog
WORKDIR /usr/src/services/catalog

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}
EXPOSE 5002
ENV NODE_ENV production

COPY ./src/services/catalog/package.json /usr/src/services/catalog/
RUN yarn --network-timeout 100000 install

COPY ./src/services/catalog/ /usr/src/services/catalog/

RUN chmod +x script.sh

ENTRYPOINT ["./script.sh"]
