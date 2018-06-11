FROM node:10.0.0-slim

RUN mkdir -p /opt/web
COPY ./ /opt/web/
RUN cd /opt/web; yarn install

EXPOSE 3000
WORKDIR /opt/web

CMD ["yarn","start"]
