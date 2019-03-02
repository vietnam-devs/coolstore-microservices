# Reference at https://github.com/DataGrip/docker-env/blob/master/mssql-server-linux/Dockerfile
FROM microsoft/mssql-server-linux:2017-latest

WORKDIR .

ENV LANG en_US.UTF-8
ENV LANGUAGE en_US:en
ENV LC_ALL en_US.UTF-8

ADD ./deploys/dockers/mssqldb/entrypoint.sh /entrypoint.sh
ADD ./deploys/dockers/mssqldb/init.sh /init.sh

RUN ["/bin/bash", "-c", "chmod +x /entrypoint.sh && chmod +x /init.sh"]

EXPOSE 1433

ENTRYPOINT ["/entrypoint.sh"]

# Tail the setup logs to trap the process
CMD ["tail -f /dev/null"]

HEALTHCHECK --interval=15s CMD /opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -Q "select 1" && grep -q "MSSQL SERVER SUCCESSFULLY STARTED" ./var/opt/mssql/log/errorlog