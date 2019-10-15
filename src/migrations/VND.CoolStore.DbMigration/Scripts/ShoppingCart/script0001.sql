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
    [UserId] uniqueidentifier NOT NULL,
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
    [IsDeleted] bit NOT NULL,
    [InventoryId] uniqueidentifier NOT NULL,
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

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Desc', N'ImagePath', N'InventoryId', N'IsDeleted', N'Name', N'Price', N'ProductId', N'Updated', N'Version') AND [object_id] = OBJECT_ID(N'[catalog].[ProductCatalogs]'))
    SET IDENTITY_INSERT [catalog].[ProductCatalogs] ON;
INSERT INTO [catalog].[ProductCatalogs] ([Id], [Created], [Desc], [ImagePath], [InventoryId], [IsDeleted], [Name], [Price], [ProductId], [Updated], [Version])
VALUES ('1c0fc113-0c71-4595-90a1-902acb26433b', '2019-10-15T06:49:14.5984707Z', N'quis nostrud exercitation ull', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'tempor incididunt ut labore et do', 638.0E0, '05233341-185a-468a-b074-00ebd08559aa', NULL, 0),
('224f3f08-e581-4b7e-8a68-784183ecf0ef', '2019-10-15T06:49:14.5988127Z', N'sin', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'm', 671.0E0, '3cb275c5-aa53-40ff-bc6a-015327053af9', NULL, 0),
('d4d05445-033f-4561-aacb-17646e04c04d', '2019-10-15T06:49:14.5988190Z', N'dolor sit amet, consectetur adipiscing e', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'ut labore et dolore magna aliqua. Ut enim ad minim ', 901.0E0, 'a162b9ee-85b4-457a-93fc-015df74201dd', NULL, 0),
('8e175f62-eee0-4e5c-a647-5c87b566f22e', '2019-10-15T06:49:14.5988240Z', N'est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididun', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'non proident, sunt in culpa qui officia deserunt mollit anim id', 661.0E0, 'ff58a71d-76a2-41f8-af44-018969694a59', NULL, 0),
('84829642-9c9b-4c84-a209-da742d9175ca', '2019-10-15T06:49:14.5988251Z', N'ipsum dolor sit amet, consectetur adipis', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'tempor incididunt ut labore ', 80.0E0, '9032b448-61f2-45f8-9e95-020961441613', NULL, 0),
('52d2459b-bb2d-4736-82eb-02f138faace1', '2019-10-15T06:49:14.5988260Z', N'officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipi', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'aliqua. Ut enim ad minim veniam, quis nostrud exercitation ul', 275.0E0, 'd16e6353-0f88-43ba-9303-0241672d6ab6', NULL, 0),
('ee4fae6f-a103-4bc9-943e-6e85fd7e3f29', '2019-10-15T06:49:14.5988271Z', N'mollit anim id est laborum.Lo', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'velit esse cillum dolore eu fugiat ', 738.0E0, '80258882-2a90-4038-ac48-0283bb0ac9b7', NULL, 0),
('7221e8d6-ae2c-4283-b685-096bb53a13fc', '2019-10-15T06:49:14.5988280Z', N'elit, sed do eiusmod tempor incididunt ut labore et dolore magna a', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'aute irure dolor in re', 51.0E0, 'a11128b0-dd82-4179-99d9-0288e22db70b', NULL, 0),
('bbcee1b4-a9cf-450d-b954-96cc0dab89f0', '2019-10-15T06:49:14.5988289Z', N'Excepteur sint occaecat cupidatat non proident, sunt in culpa qui ', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsu', 847.0E0, 'e96a0646-6508-4e40-a035-0294e3b6a017', NULL, 0),
('12341364-fe69-4239-8020-c008488b17ce', '2019-10-15T06:49:14.5988298Z', N'voluptate velit esse cillum dolore eu fugiat nulla pariat', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna a', 2.0E0, 'd39650d3-7929-4702-bcb9-02978d2c2711', NULL, 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Desc', N'ImagePath', N'InventoryId', N'IsDeleted', N'Name', N'Price', N'ProductId', N'Updated', N'Version') AND [object_id] = OBJECT_ID(N'[catalog].[ProductCatalogs]'))
    SET IDENTITY_INSERT [catalog].[ProductCatalogs] OFF;

GO

CREATE INDEX [IX_CartItems_CurrentCartId] ON [cart].[CartItems] ([CurrentCartId]);

GO

CREATE UNIQUE INDEX [IX_ProductCatalogIds_CurrentCartItemId] ON [cart].[ProductCatalogIds] ([CurrentCartItemId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191015064914_InitialShoppingCartDb', N'3.0.0');

GO

