# Reference at https://github.com/DataGrip/docker-env/blob/master/mssql-server-linux/Dockerfile
FROM microsoft/mssql-server-linux:2017-latest

WORKDIR .

ENV LANG en_US.UTF-8
ENV LANGUAGE en_US:en
ENV LC_ALL en_US.UTF-8

ADD ./src/services/mssql-db/entrypoint.sh /entrypoint.sh
ADD ./src/services/mssql-db/init.sh /init.sh

RUN ["/bin/bash", "-c", "chmod +x /entrypoint.sh && chmod +x /init.sh"]

EXPOSE 1433

ENTRYPOINT ["/entrypoint.sh"]