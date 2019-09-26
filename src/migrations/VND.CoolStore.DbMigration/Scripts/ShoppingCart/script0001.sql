IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF SCHEMA_ID(N'cart') IS NULL EXEC(N'CREATE SCHEMA [cart];');

GO

IF SCHEMA_ID(N'catalog') IS NULL EXEC(N'CREATE SCHEMA [catalog];');

GO

CREATE TABLE [cart].[Carts] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NULL,
    [Version] int NOT NULL,
    [CartItemTotal] float NOT NULL,
    [CartItemPromoSavings] float NOT NULL,
    [ShippingTotal] float NOT NULL,
    [ShippingPromoSavings] float NOT NULL,
    [CartTotal] float NOT NULL,
    [IsCheckout] bit NOT NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [catalog].[ProductCatalogs] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NULL,
    [Version] int NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [Price] float NOT NULL,
    [Desc] nvarchar(max) NULL,
    [ImagePath] nvarchar(max) NULL,
    CONSTRAINT [PK_ProductCatalogs] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [cart].[CartItems] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NULL,
    [Quantity] int NOT NULL,
    [PromoSavings] float NOT NULL,
    [CurrentCartId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CartItems_Carts_CurrentCartId] FOREIGN KEY ([CurrentCartId]) REFERENCES [cart].[Carts] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [cart].[ProductCatalogIds] (
    [Id] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [CurrentCartItemId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_ProductCatalogIds] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductCatalogIds_CartItems_CurrentCartItemId] FOREIGN KEY ([CurrentCartItemId]) REFERENCES [cart].[CartItems] ([Id]) ON DELETE CASCADE
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Desc', N'ImagePath', N'Name', N'Price', N'ProductId', N'Updated', N'Version') AND [object_id] = OBJECT_ID(N'[catalog].[ProductCatalogs]'))
    SET IDENTITY_INSERT [catalog].[ProductCatalogs] ON;
INSERT INTO [catalog].[ProductCatalogs] ([Id], [Created], [Desc], [ImagePath], [Name], [Price], [ProductId], [Updated], [Version])
VALUES ('df79f461-e985-4ebe-bf65-922bc85a6f8d', '2019-09-26T17:22:59.7466695Z', N'quis nostrud exercitation ull', N'https://picsum.photos/1200/900?image=1', N'tempor incididunt ut labore et do', 638.0E0, '05233341-185a-468a-b074-00ebd08559aa', NULL, 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Desc', N'ImagePath', N'Name', N'Price', N'ProductId', N'Updated', N'Version') AND [object_id] = OBJECT_ID(N'[catalog].[ProductCatalogs]'))
    SET IDENTITY_INSERT [catalog].[ProductCatalogs] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Desc', N'ImagePath', N'Name', N'Price', N'ProductId', N'Updated', N'Version') AND [object_id] = OBJECT_ID(N'[catalog].[ProductCatalogs]'))
    SET IDENTITY_INSERT [catalog].[ProductCatalogs] ON;
INSERT INTO [catalog].[ProductCatalogs] ([Id], [Created], [Desc], [ImagePath], [Name], [Price], [ProductId], [Updated], [Version])
VALUES ('be559748-37b4-488f-bb31-deae8109895b', '2019-09-26T17:22:59.7469546Z', N'sin', N'https://picsum.photos/1200/900?image=1', N'm', 671.0E0, '3cb275c5-aa53-40ff-bc6a-015327053af9', NULL, 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Desc', N'ImagePath', N'Name', N'Price', N'ProductId', N'Updated', N'Version') AND [object_id] = OBJECT_ID(N'[catalog].[ProductCatalogs]'))
    SET IDENTITY_INSERT [catalog].[ProductCatalogs] OFF;

GO

CREATE INDEX [IX_CartItems_CurrentCartId] ON [cart].[CartItems] ([CurrentCartId]);

GO

CREATE UNIQUE INDEX [IX_ProductCatalogIds_CurrentCartItemId] ON [cart].[ProductCatalogIds] ([CurrentCartItemId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190926172300_InitialShoppingCartDb', N'3.0.0');

GO

