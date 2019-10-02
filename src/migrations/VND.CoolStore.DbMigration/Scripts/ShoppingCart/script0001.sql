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
VALUES ('5054021b-d151-473d-ab12-f8f81f194e22', '2019-10-02T08:10:06.6482773Z', N'quis nostrud exercitation ull', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'tempor incididunt ut labore et do', 638.0E0, '05233341-185a-468a-b074-00ebd08559aa', NULL, 0),
('56170822-c912-4a17-bbc0-4e6389a0c4db', '2019-10-02T08:10:06.6491057Z', N'sin', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'm', 671.0E0, '3cb275c5-aa53-40ff-bc6a-015327053af9', NULL, 0),
('d2549324-5299-413f-922b-f707f108e739', '2019-10-02T08:10:06.6491592Z', N'dolor sit amet, consectetur adipiscing e', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'ut labore et dolore magna aliqua. Ut enim ad minim ', 901.0E0, 'a162b9ee-85b4-457a-93fc-015df74201dd', NULL, 0),
('32945efe-294c-4c53-80b7-224cb08c8069', '2019-10-02T08:10:06.6491614Z', N'est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididun', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'non proident, sunt in culpa qui officia deserunt mollit anim id', 661.0E0, 'ff58a71d-76a2-41f8-af44-018969694a59', NULL, 0),
('3e5cc141-68bb-42b1-b176-70cc58bccedd', '2019-10-02T08:10:06.6491635Z', N'ipsum dolor sit amet, consectetur adipis', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'tempor incididunt ut labore ', 80.0E0, '9032b448-61f2-45f8-9e95-020961441613', NULL, 0),
('1016202d-ebc5-41f4-9728-8fd7289457fd', '2019-10-02T08:10:06.6491666Z', N'officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipi', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'aliqua. Ut enim ad minim veniam, quis nostrud exercitation ul', 275.0E0, 'd16e6353-0f88-43ba-9303-0241672d6ab6', NULL, 0),
('c443fcf4-5aed-44fe-a45f-6a4c6a7b944e', '2019-10-02T08:10:06.6491683Z', N'mollit anim id est laborum.Lo', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'velit esse cillum dolore eu fugiat ', 738.0E0, '80258882-2a90-4038-ac48-0283bb0ac9b7', NULL, 0),
('176b607d-9228-438a-bd7b-cc6c1d2e03fd', '2019-10-02T08:10:06.6491762Z', N'elit, sed do eiusmod tempor incididunt ut labore et dolore magna a', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'aute irure dolor in re', 51.0E0, 'a11128b0-dd82-4179-99d9-0288e22db70b', NULL, 0),
('ed08787c-088d-41ac-a8f2-87a1b4c34f96', '2019-10-02T08:10:06.6491780Z', N'Excepteur sint occaecat cupidatat non proident, sunt in culpa qui ', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsu', 847.0E0, 'e96a0646-6508-4e40-a035-0294e3b6a017', NULL, 0),
('da5aa144-ae6c-4a29-897d-4f4bd770ff55', '2019-10-02T08:10:06.6491797Z', N'voluptate velit esse cillum dolore eu fugiat nulla pariat', N'https://picsum.photos/1200/900?image=1', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna a', 2.0E0, 'd39650d3-7929-4702-bcb9-02978d2c2711', NULL, 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Desc', N'ImagePath', N'InventoryId', N'IsDeleted', N'Name', N'Price', N'ProductId', N'Updated', N'Version') AND [object_id] = OBJECT_ID(N'[catalog].[ProductCatalogs]'))
    SET IDENTITY_INSERT [catalog].[ProductCatalogs] OFF;

GO

CREATE INDEX [IX_CartItems_CurrentCartId] ON [cart].[CartItems] ([CurrentCartId]);

GO

CREATE UNIQUE INDEX [IX_ProductCatalogIds_CurrentCartItemId] ON [cart].[ProductCatalogIds] ([CurrentCartItemId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191002081007_InitialShoppingCartDb', N'3.0.0');

GO

