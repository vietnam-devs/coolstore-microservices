# Data Migration

```bash
$ dotnet tool install -g dotnet-ef --version 3.0.0-*
```

```bash
$ dotnet build
```

```bash
$ dotnet ef migrations add InitialShoppingCartDb -c ShoppingCartDataContext -o Data/Migrations
$ dotnet ef migrations add InitialMessageDb -c MessagingDataContext -o Data/Migrations
```

```bash
$ dotnet ef migrations script -c ShoppingCartDataContext --output ./../../../migrations/VND.CoolStore.DbMigration/Scripts/ShoppingCart/script0001.sql
$ dotnet ef migrations script -c MessagingDataContext --output ./../../../migrations/VND.CoolStore.DbMigration/Scripts/ShoppingCart/script0002.sql
```
