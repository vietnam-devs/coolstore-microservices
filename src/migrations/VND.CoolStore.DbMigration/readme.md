```bash
$ dotnet run shoppingcart
$ dotnet run productcatalog
```

- Development

```json
{
  "ConnectionStrings": {
    "shoppingcart": "Data Source=localhost,1401;Initial Catalog=ShoppingCartDb;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=True;",
    "productcatalog": "Data Source=localhost,1401;Initial Catalog=ProductCatalogDb;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=True",
    "inventory": "Data Source=localhost;Initial Catalog=InventoryDb;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=True"
  }
}
```

- Production

```json
{
  "ConnectionStrings": {
    "shoppingcart": "Data Source=sqlserver.data,1433;Initial Catalog=ShoppingCartDb;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=True;",
    "productcatalog": "Data Source=sqlserver.data,1433;Initial Catalog=ProductCatalogDb;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=True",
    "inventory": "Data Source=sqlserver.data,1433;Initial Catalog=InventoryDb;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=True"
  }
}
```