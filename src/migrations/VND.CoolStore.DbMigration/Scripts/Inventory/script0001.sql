IF SCHEMA_ID(N'inventory') IS NULL EXEC(N'CREATE SCHEMA [inventory];');

GO

CREATE TABLE [inventory].[Inventories] (
  Id uniqueidentifier NOT NULL,
  [Created] datetime2 NOT NULL,
  [Updated] datetime2 NOT NULL,
  [Version] int NOT NULL,
	[Location] varchar(100) NOT NULL,
	[Description] varchar(500) NULL,
	[Website] varchar(100) NOT NULL,
	CONSTRAINT [PK_Inventories] PRIMARY KEY ([Id])
) GO

INSERT INTO InventoryDb.inventory.Inventories (Id,Created,Updated,Version,Location,Description,Website) VALUES 
('90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD','2002-01-24 07:50:18.085','2011-12-26 03:41:43.805',23386,'Vietnam','This store sells electronic gadgets','https://coolstore-vn.com')
,('B8B62196-6369-409D-B709-11C112DD023F','1987-01-06 01:58:27.461','2010-01-29 20:28:07.761',29744,'Seattle','This store sells food and beverage products','https://coolstore-sea.com')
,('EC186DDF-F430-44EC-84E5-205C93D84F14','2013-01-18 04:25:34.804','1983-10-20 03:15:19.268',12511,'San Francisco','This store sells food and beverage products','https://coolstore-san.com')
;

GO
