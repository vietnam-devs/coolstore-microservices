#!/bin/bash

echo =============== WAITING FOR MSSQL ==========================
#waiting for mssql to start
export STATUS=0
i=0
while [[ $STATUS -eq 0 ]] || [[ $i -lt 90 ]]; do
	sleep 1
	i=$((i+1))
	export STATUS=$(grep 'SQL Server is now ready for client connections' /var/opt/mssql/log/errorlog | wc -l)
done

echo =============== MSSQL STARTED ==========================

if [ ! -z $MSSQL_USER ]; then
	echo "MSSQL_USER: $MSSQL_USER"
else
	MSSQL_USER=cs
	echo "MSSQL_USER: $MSSQL_USER"
fi

if [ ! -z $MSSQL_PASSWORD ]; then
	echo "MSSQL_PASSWORD: $MSSQL_PASSWORD"
else
	MSSQL_PASSWORD=P@ssw0rd
	echo "MSSQL_PASSWORD: $MSSQL_PASSWORD"
fi

if [ ! -z $MSSQL_DB ]; then
	echo "MSSQL_DB: $MSSQL_DB"
else
	MSSQL_DB=maindb
	echo "MSSQL_DB: $MSSQL_DB"
fi

echo =============== CREATING INIT DATA ==========================

cd /opt/mssql/

cat <<-EOSQL > init.sql
CREATE DATABASE $MSSQL_DB;
GO
USE $MSSQL_DB;
GO
CREATE LOGIN $MSSQL_USER WITH PASSWORD = '$MSSQL_PASSWORD';
GO
CREATE USER $MSSQL_USER FOR LOGIN $MSSQL_USER;
GO
ALTER SERVER ROLE sysadmin ADD MEMBER [$MSSQL_USER];
GO
EOSQL

cd /opt/mssql-tools/bin/
./sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -t 30 -i"/opt/mssql/init.sql" -o"/opt/mssql/initout.log"

echo "=============== INIT DATA CREATED ==========================" | tee -a /var/opt/mssql/log/errorlog
echo "=============== MSSQL SERVER SUCCESSFULLY STARTED ==========================" | tee -a /var/opt/mssql/log/errorlog

#trap
while [ "$END" == '' ]; do
    sleep 1
    trap "END=1" INT TERM
done