```bash
$ dotnet tool install -g dotnet-ef --version 3.0.0-*
```

```bash
$ dotnet build
```

```bash
$ dotnet ef migrations add InitialDb
```

```bash
$ dotnet ef migrations script --output ./../../../migrations/VND.CoolStore.DbMigration/Scripts/ShoppingCart/script0001.sql
```
