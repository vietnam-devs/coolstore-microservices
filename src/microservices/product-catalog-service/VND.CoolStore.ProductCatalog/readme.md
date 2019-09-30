# Data Migration

```bash
$ dotnet build
```

```bash
$ dotnet ef migrations add InitialMessageDb -c MessagingDataContext -o Data/Migrations
```

```bash
$ dotnet ef migrations script -c MessagingDataContext --output ./../../../migrations/VND.CoolStore.DbMigration/Scripts/ProductCatalog/script0002.sql
```