FROM mysql:8.0.12

# MYSQL_ROOT_PASSWORD must be supplied as an env var

COPY ./deploys/dockers/mysqldb/mysqldb-init.sql /docker-entrypoint-initdb.d
