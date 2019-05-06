FROM mysql:5.7.14

# MYSQL_ROOT_PASSWORD must be supplied as an env var

COPY ./deploys/dockers/mysqldb/mysqldb-init.sql /docker-entrypoint-initdb.d
