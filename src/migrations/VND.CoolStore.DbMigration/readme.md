```bash
$ docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd" --name sqlserver -p 1401:1433 -d mcr.microsoft.com/mssql/server:2017-latest
```

```bash
$ dotnet run shoppingcart productcatalog
```
