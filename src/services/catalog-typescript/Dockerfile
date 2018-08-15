FROM node:alpine

#RUN mkdir -p /usr/src/services/catalog
#RUN mkdir -p /usr/src/services/catalog/dist
#WORKDIR /usr/src/services/catalog

ARG service_version
ENV SERVICE_VERSION ${service_version:-v1}
EXPOSE 5002
ENV NODE_ENV production

RUN mkdir -p /app/dist
WORKDIR /app
COPY ./src/services/catalog-typescript/ .
# RUN npm set progress=false && npm config set depth 0 && npm cache clean --force
RUN npm install --silent --save-dev -g rimraf cross-env
RUN yarn install
#&& mkdir -p /app/dist && cp -R ./node_modules ./app

#COPY ./src/services/catalog-typescript/ .

RUN chmod +x ./script.sh
ENTRYPOINT ["./script.sh"]
