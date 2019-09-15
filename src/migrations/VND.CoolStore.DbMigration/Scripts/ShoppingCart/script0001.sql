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

CREATE TABLE [ProductCatalogId] (
    [Id] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_ProductCatalogId] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [cart].[Carts] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NOT NULL,
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

CREATE TABLE [cart].[CartItems] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NOT NULL,
    [Quantity] int NOT NULL,
    [Price] float NOT NULL,
    [PromoSavings] float NOT NULL,
    [CartId] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CartItems_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [cart].[Carts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItems_ProductCatalogId_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [ProductCatalogId] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_CartItems_CartId] ON [cart].[CartItems] ([CartId]);

GO

CREATE INDEX [IX_CartItems_ProductId] ON [cart].[CartItems] ([ProductId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190915051829_InitialDb', N'3.0.0-preview9.19423.6');

GO

