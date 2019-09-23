IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF SCHEMA_ID(N'message') IS NULL EXEC(N'CREATE SCHEMA [message];');

GO

CREATE TABLE [message].[Outboxes] (
    [Id] uniqueidentifier NOT NULL,
    [Created] datetime2 NOT NULL,
    [Updated] datetime2 NOT NULL,
    [Version] int NOT NULL,
    [OccurredOn] datetime2 NOT NULL,
    [Type] nvarchar(max) NULL,
    [Data] nvarchar(max) NULL,
    [ProcessedDate] datetime2 NULL,
    CONSTRAINT [PK_Outboxes] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190923123236_InitialMessageDb', N'3.0.0-rc1.19456.14');

GO

