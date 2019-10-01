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
('90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD','2002-01-24 07:50:18.085','2011-12-26 03:41:43.805',23386,'elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ','adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. U','http://coolstore1.com')
,('B8B62196-6369-409D-B709-11C112DD023F','1987-01-06 01:58:27.461','2010-01-29 20:28:07.761',29744,'irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat n','dolor in reprehenderit in voluptate velit esse cillum dolore e','http://coolstore2.com')
,('EC186DDF-F430-44EC-84E5-205C93D84F14','2013-01-18 04:25:34.804','1983-10-20 03:15:19.268',12511,'sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mol','aute irure dolor in reprehenderit in voluptate veli','http://coolstore3.com')
,('46221F5A-1422-4063-88A1-24F5FAA1A511','1981-04-14 05:18:06.534','1992-08-18 16:58:56.904',4387,'ut labore et dolore magna aliqua','Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi u','http://coolstore4.com')
,('302ED8CA-6D49-4B0D-AB93-29D451BE6A18','2004-12-03 01:54:26.307','2006-08-05 02:04:10.623',3201,'non proident, s','et dolore','http://coolstore5.com')
,('D3762560-1B9A-4C97-91F6-326D14E60EF1','1990-05-26 19:10:18.512','1982-06-01 11:51:28.780',21695,'eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident,','proident, sunt in culp','http://coolstore6.com')
,('2927462C-AF4E-4B60-B23B-33583C9A3D29','1997-01-23 22:53:35.299','2016-11-30 11:47:44.137',12796,'ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate ','adipiscing elit, sed do eiusmod te','http://coolstore7.com')
,('058F7BA1-1C52-44E1-B724-386EFF495AA8','1998-02-26 14:04:35.575','2015-01-09 04:32:37.803',3761,'tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud ex','Ut enim ad minim veniam, qui','http://coolstore8.com')
,('FEEEFA8D-1FA8-403A-9891-3A53F580EF60','2002-03-02 13:32:52.918','2007-12-19 21:16:48.581',1825,'dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incidi','non p','http://coolstore9.com')
,('A545AC82-61CB-4697-8EC4-3AF67E09EB6E','2019-02-21 05:49:23.747','1991-08-14 08:40:02.954',9642,'proident, sunt in culpa qui offic','tempor incididunt ut labore et dolore magna aliq','http://coolstore10.com')
;

GO
