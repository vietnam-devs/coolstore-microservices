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
VALUES ('2bff915e-5ae3-4399-bcd9-d9b41d969482', '2019-11-02T11:03:06.2941501Z', N'IPhone 8', N'https://picsum.photos/1200/900?image=1', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'IPhone 8', 900.0E0, 'ba16da71-c7dd-4eac-9ead-5c2c2244e69f', NULL, 0),
('ab7b752a-ac88-4053-be5e-ed0e4c4a896c', '2019-11-02T11:03:06.2960363Z', N'Implt/repl carddefib tot', N'https://picsum.photos/1200/900?image=25', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Soup - Campbells Chili', 3294.0E0, 'b243a35d-120a-4db3-ad12-7b3fa80e6391', NULL, 0),
('04404f65-0648-4967-8896-ffae967e721c', '2019-11-02T11:03:06.2960325Z', N'Oth chest cage rep/plast', N'https://picsum.photos/1200/900?image=24', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Lotus Leaves', 1504.0E0, 'e88e07f8-358d-48f7-b17c-8a16458f9c0a', NULL, 0),
('8124fd52-5565-47b6-b16b-1a9ef6e67b2a', '2019-11-02T11:03:06.2960286Z', N'Excision of wrist NEC', N'https://picsum.photos/1200/900?image=23', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Mushroom - Lg - Cello', 3318.0E0, '89b46ea8-b9a6-40e5-8df3-dba1095695f7', NULL, 0),
('c8f4e2e5-2fbc-4bdc-be11-1a20b1aa3ae3', '2019-11-02T11:03:06.2960249Z', N'Appendiceal ops NEC', N'https://picsum.photos/1200/900?image=22', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Mix - Cocktail Ice Cream', 232.0E0, '3b69e116-9dd6-400f-96ce-9911f4f6ac8b', NULL, 0),
('9ad64cce-30b0-47aa-808d-ba74aa4c22ee', '2019-11-02T11:03:06.2960207Z', N'Proximal gastrectomy', N'https://picsum.photos/1200/900?image=21', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Ice Cream Bar - Oreo Cone', 2236.0E0, '6b8d0110-e3e8-4727-a51e-06f38864e464', NULL, 0),
('91e4bca2-4b63-429a-afb1-71ec29d9f6d3', '2019-11-02T11:03:06.2960143Z', N'Hepatic lobectomy', N'https://picsum.photos/1200/900?image=20', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Beef - Shank', 3196.0E0, 'c3770b10-dd0f-4b1c-83aa-d424c175c087', NULL, 0),
('1460975d-b66c-4ef2-b513-6dc1f0697fc9', '2019-11-02T11:03:06.2960107Z', N'Interat ven retrn transp', N'https://picsum.photos/1200/900?image=20', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Milk - Skim', 3310.0E0, '1adbc55a-4354-4205-b96d-c95e2dc806f4', NULL, 0),
('7a211d61-c5da-4891-af3b-decef054b1c7', '2019-11-02T11:03:06.2960071Z', N'Plastic rep ext ear NEC', N'https://picsum.photos/1200/900?image=19', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Pasta - Cappellini, Dry', 3305.0E0, 'fac2ccc6-2c3f-4c1e-acbd-5354ba0873fb', NULL, 0),
('4fb812db-8b7e-4d74-bbc3-ffd6c662406f', '2019-11-02T11:03:06.2960035Z', N'Chng hnd mus/ten lng NEC', N'https://picsum.photos/1200/900?image=18', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Crab - Dungeness, Whole, live', 1665.0E0, 'cfc5cff8-ab2a-4c70-994d-5ab8ccb7cb0d', NULL, 0),
('c9e864dc-a949-4b6c-b5e1-8fc941ba8efb', '2019-11-02T11:03:06.2959996Z', N'Skull plate removal', N'https://picsum.photos/1200/900?image=17', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Oil - Olive', 1124.0E0, '97ad5bf4-d153-41c5-a6e0-6d0bfbbb4f67', NULL, 0),
('cfc3bbe3-7576-4f99-89e7-6b1ff16e064e', '2019-11-02T11:03:06.2959954Z', N'Vessel operation NEC', N'https://picsum.photos/1200/900?image=16', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Beef - Tenderloin Tails', 967.0E0, '22112bb2-c324-4860-8eb9-9c78853a52a5', NULL, 0),
('6aad4645-5b3d-4b42-bb81-eb53e916f7d2', '2019-11-02T11:03:06.2959853Z', N'Tendon excision for grft', N'https://picsum.photos/1200/900?image=15', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Tarragon - Primerba, Paste', 642.0E0, '85b9767c-1a09-4c33-8e77-faa25f1d69e1', NULL, 0),
('cb2cab44-3c47-4f4b-b2e5-36344c75b4e3', '2019-11-02T11:03:06.2959816Z', N'Opn/oth part gastrectomy', N'https://picsum.photos/1200/900?image=14', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Ecolab - Balanced Fusion', 1769.0E0, 'cbe43158-2010-4cb1-a8de-f00da16a7362', NULL, 0),
('fe44ccd1-7a97-4e68-809f-d76cc0a58849', '2019-11-02T11:03:06.2959780Z', N'Toxicology-endocrine', N'https://picsum.photos/1200/900?image=13', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Godiva White Chocolate', 2067.0E0, 'f92bfa6a-2522-4234-a7f1-9004596a4a85', NULL, 0),
('af7398b6-5b33-49d0-b00b-666c5f86c44d', '2019-11-02T11:03:06.2959743Z', N'Oth thorac op thymus NOS', N'https://picsum.photos/1200/900?image=12', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Lettuce - Boston Bib', 3453.0E0, '71c46659-9560-4d6a-ac18-893477ed6662', NULL, 0),
('b118e886-3bdf-4e00-8e53-62416aea1438', '2019-11-02T11:03:06.2959707Z', N'Other bone dx proc NEC', N'https://picsum.photos/1200/900?image=11', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Cheese - Swiss', 975.0E0, '3a0a0a89-9b3a-4046-bf2b-deee64a764d2', NULL, 0),
('1b9a1364-0b3b-40b9-a22e-946629985bda', '2019-11-02T11:03:06.2959670Z', N'Remove bladder stimulat', N'https://picsum.photos/1200/900?image=10', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Oranges - Navel, 72', 1731.0E0, '297c5959-4808-4f40-8d6a-4a899505e1f7', NULL, 0),
('2368006a-66f1-4537-a8ce-054dc828abaf', '2019-11-02T11:03:06.2959626Z', N'Removal of FB NOS', N'https://picsum.photos/1200/900?image=9', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Hersey Shakes', 2441.0E0, '386b04c6-303a-4840-8a51-d92b1ea2d339', NULL, 0),
('8fb095ab-c964-47a9-8199-da74813e478c', '2019-11-02T11:03:06.2959551Z', N'Tibia/fibula inj op NOS', N'https://picsum.photos/1200/900?image=8', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Wonton Wrappers', 2200.0E0, '2d2245e4-213a-49de-93d3-79e9439400f5', NULL, 0),
('9a268fe5-4588-4e93-ab51-e327f43ce480', '2019-11-02T11:03:06.2959514Z', N'Open periph nerve biopsy', N'https://picsum.photos/1200/900?image=7', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Bag - Regular Kraft 20 Lb', 2147.0E0, 'fee1fc67-7469-4490-b418-47f4732de53f', NULL, 0),
('9cb36c66-2172-46ad-836a-14de6ee1c2fb', '2019-11-02T11:03:06.2959477Z', N'Fiber-optic bronchoscopy', N'https://picsum.photos/1200/900?image=6', 'ec186ddf-f430-44ec-84e5-205c93d84f14', CAST(0 AS bit), N'Bread - White, Unsliced', 2809.0E0, '6a0e6d20-8bcc-450f-bc5c-b8f727083dcd', NULL, 0),
('67e7acb3-05b2-4b3f-8df7-0940fc95338e', '2019-11-02T11:03:06.2959429Z', N'Mastoidectomy revision', N'https://picsum.photos/1200/900?image=5', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Cheese - Camembert', 253.0E0, 'a4811778-5070-4d70-8a9c-e6cb70dfcca4', NULL, 0),
('ef59b997-c867-44bd-819e-960bfec71ef1', '2019-11-02T11:03:06.2959391Z', N'Asus UX370U i7 8550U (C4217TS)', N'https://picsum.photos/1200/900?image=4', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'Asus UX370U i7 8550U (C4217TS)', 500.0E0, 'ffd60654-1802-48bd-b4c3-d49831a8ab2c', NULL, 0),
('e757dd63-3cfd-42a3-bfd4-92241afe4f9a', '2019-11-02T11:03:06.2959341Z', N'MacBook Pro 2019', N'https://picsum.photos/1200/900?image=3', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'MacBook Pro 2019', 4000.0E0, 'b8f0a771-339f-4602-a862-f7a51afd5b79', NULL, 0),
('1a2b2109-33db-4a3b-96f0-88394cb68328', '2019-11-02T11:03:06.2959080Z', N'IPhone X', N'https://picsum.photos/1200/900?image=2', '90c9479e-a11c-4d6d-aaaa-0405b6c0efcd', CAST(0 AS bit), N'IPhone X', 1000.0E0, '13d02035-2286-4055-ad2d-6855a60efbbb', NULL, 0),
('041e4f4c-0a3a-4016-849f-5124d04775d5', '2019-11-02T11:03:06.2960399Z', N'Wound catheter irrigat', N'https://picsum.photos/1200/900?image=26', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Longos - Penne With Pesto', 3639.0E0, '6e3ac253-517d-48e5-96ad-800451f8591c', NULL, 0),
('fd85394c-420e-486e-a937-08d529ade7e1', '2019-11-02T11:03:06.2960594Z', N'Abdomen wall repair NEC', N'https://picsum.photos/1200/900?image=27', 'b8b62196-6369-409d-b709-11c112dd023f', CAST(0 AS bit), N'Prunes - Pitted', 1191.0E0, '4693520a-2b14-4d90-8b64-541575511382', NULL, 0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Created', N'Desc', N'ImagePath', N'InventoryId', N'IsDeleted', N'Name', N'Price', N'ProductId', N'Updated', N'Version') AND [object_id] = OBJECT_ID(N'[catalog].[ProductCatalogs]'))
    SET IDENTITY_INSERT [catalog].[ProductCatalogs] OFF;

GO

CREATE INDEX [IX_CartItems_CurrentCartId] ON [cart].[CartItems] ([CurrentCartId]);

GO

CREATE UNIQUE INDEX [IX_ProductCatalogIds_CurrentCartItemId] ON [cart].[ProductCatalogIds] ([CurrentCartItemId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191102110306_InitialShoppingCartDb', N'3.0.0');

GO

