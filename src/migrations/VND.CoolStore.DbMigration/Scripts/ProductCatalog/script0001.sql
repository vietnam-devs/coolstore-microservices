IF SCHEMA_ID(N'catalog') IS NULL EXEC(N'CREATE SCHEMA [catalog];');

GO

CREATE TABLE [catalog].[Products] (
  Id uniqueidentifier NOT NULL,
  [Created] datetime2 NOT NULL,
  [Updated] datetime2 NOT NULL,
  [Version] int NOT NULL,
	[Name] varchar(100) NOT NULL,
	[Description] varchar(500) NULL,
	[Price] float NOT NULL,
	[ImageUrl] varchar(500) NOT NULL,
  [InventoryId] uniqueidentifier NOT NULL,
  [IsDeleted] bit NOT NULL,
	CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
) GO

INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl, InventoryId, IsDeleted) VALUES
('05233341-185A-468A-B074-00EBD08559AA','1991-08-02 15:33:00.040','2008-03-09 03:05:15.522',4612,'tempor incididunt ut labore et do','quis nostrud exercitation ull',638,'https://picsum.photos/1200/900?image=1', '90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD',CAST(0 AS bit))
,('3CB275C5-AA53-40FF-BC6A-015327053AF9','2016-05-10 05:01:44.437','2008-09-12 15:10:31.142',25414,'m','sin',671,'https://picsum.photos/1200/900?image=1', 'EC186DDF-F430-44EC-84E5-205C93D84F14', CAST(0 AS bit))
,('A162B9EE-85B4-457A-93FC-015DF74201DD','1999-03-05 05:43:42.219','2006-10-27 13:42:49.099',10890,'ut labore et dolore magna aliqua. Ut enim ad minim ','dolor sit amet, consectetur adipiscing e',901,'https://picsum.photos/1200/900?image=1', 'EC186DDF-F430-44EC-84E5-205C93D84F14', CAST(0 AS bit))
,('FF58A71D-76A2-41F8-AF44-018969694A59','1999-01-25 00:11:04.022','2006-12-03 03:01:37.382',17558,'non proident, sunt in culpa qui officia deserunt mollit anim id','est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididun',661,'https://picsum.photos/1200/900?image=1', 'B8B62196-6369-409D-B709-11C112DD023F', CAST(0 AS bit))
,('9032B448-61F2-45F8-9E95-020961441613','2005-05-17 08:39:00.444','2014-08-26 23:49:48.465',7106,'tempor incididunt ut labore ','ipsum dolor sit amet, consectetur adipis',80,'https://picsum.photos/1200/900?image=1', '46221F5A-1422-4063-88A1-24F5FAA1A511', CAST(0 AS bit))
,('D16E6353-0F88-43BA-9303-0241672D6AB6','2016-12-13 01:54:58.027','2017-01-08 15:29:22.082',12113,'aliqua. Ut enim ad minim veniam, quis nostrud exercitation ul','officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipi',275,'https://picsum.photos/1200/900?image=1', '302ED8CA-6D49-4B0D-AB93-29D451BE6A18', CAST(0 AS bit))
,('80258882-2A90-4038-AC48-0283BB0AC9B7','2003-09-27 23:38:43.044','2010-09-18 14:00:34.626',1303,'velit esse cillum dolore eu fugiat ','mollit anim id est laborum.Lo',738,'https://picsum.photos/1200/900?image=1', '90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD', CAST(0 AS bit))
,('A11128B0-DD82-4179-99D9-0288E22DB70B','2011-03-19 15:43:25.445','1995-04-01 09:47:41.323',1182,'aute irure dolor in re','elit, sed do eiusmod tempor incididunt ut labore et dolore magna a',51,'https://picsum.photos/1200/900?image=1', '90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD', CAST(0 AS bit))
,('E96A0646-6508-4E40-A035-0294E3B6A017','2003-01-01 08:19:12.071','1980-12-03 15:01:03.026',26391,'cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsu','Excepteur sint occaecat cupidatat non proident, sunt in culpa qui ',847,'https://picsum.photos/1200/900?image=1', 'B8B62196-6369-409D-B709-11C112DD023F', CAST(0 AS bit))
,('D39650D3-7929-4702-BCB9-02978D2C2711','1989-07-28 11:42:10.666','1988-01-15 21:24:41.896',11689,'consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna a','voluptate velit esse cillum dolore eu fugiat nulla pariat',2,'https://picsum.photos/1200/900?image=1', '90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD', CAST(0 AS bit))
;
INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl, InventoryId, IsDeleted) VALUES
('E2207EA8-F965-41F9-ADC1-02B2DB6BCEDA','1980-02-20 19:44:23.924','1981-02-17 03:20:56.856',20155,'dolor in','irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Except',985,'https://picsum.photos/1200/900?image=1', '46221F5A-1422-4063-88A1-24F5FAA1A511', CAST(0 AS bit))
,('B5354991-897A-4B34-BC5D-02DE2DEF59A9','2011-08-21 09:27:19.085','1993-05-12 19:58:14.319',8202,'min','fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa ',768,'https://picsum.photos/1200/900?image=1', '90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD', CAST(0 AS bit))
,('88D41190-642A-4612-A010-030B2DBFD9C1','1984-09-29 12:43:40.750','1995-05-22 15:23:36.230',5446,'ut labore et dolore magna aliqua. Ut enim ad min','cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proid',504,'https://picsum.photos/1200/900?image=1', 'B8B62196-6369-409D-B709-11C112DD023F', CAST(0 AS bit))
,('46CF0EAA-E063-441C-8370-0378F75E2321','2009-06-22 03:58:18.671','2019-09-24 08:36:42.855',28565,'anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, se','dolore eu fugiat nulla par',432,'https://picsum.photos/1200/900?image=1', 'EC186DDF-F430-44EC-84E5-205C93D84F14', CAST(0 AS bit))
,('9A1F6596-9B5A-4942-935C-039B37235DDD','1993-07-19 18:00:12.268','1988-08-26 10:59:03.959',8559,'aute irure dolor in reprehenderit in volupta','irure dolor in reprehenderit in voluptate veli',214,'https://picsum.photos/1200/900?image=1', 'D3762560-1B9A-4C97-91F6-326D14E60EF1', CAST(0 AS bit))
,('4FDF8E83-736D-493A-9500-03A80A08E086','1985-01-29 08:40:58.052','1993-09-06 02:01:38.318',15506,'Duis aute irure dolor in reprehenderit in voluptate veli','aliqua. Ut enim ad minim veniam, quis nos',684,'https://picsum.photos/1200/900?image=1', '90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD', CAST(0 AS bit))
,('F75197D1-2C04-4950-8E40-03D69B1DCB8C','1992-07-03 17:49:58.096','1998-02-09 13:10:05.477',32224,'ullamco laboris nisi ut aliqui','ex ea commodo consequat. Dui',637,'https://picsum.photos/1200/900?image=1', 'EC186DDF-F430-44EC-84E5-205C93D84F14', CAST(0 AS bit))
,('B5BB05B9-98D9-416E-AC4E-040FDC7991A3','1990-01-19 01:22:33.299','2017-12-13 17:22:41.009',16645,'sint o','laboris nisi ut aliquip ex ea commodo conseq',351,'https://picsum.photos/1200/900?image=1', '2927462C-AF4E-4B60-B23B-33583C9A3D29', CAST(0 AS bit))
,('CEF8483A-5546-41A5-87E8-04D0C05447A1','1994-05-12 21:36:07.716','2017-08-31 05:34:12.297',14785,'officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing eli','Excepteur sint occaecat cupidatat non pr',306,'https://picsum.photos/1200/900?image=1', 'B8B62196-6369-409D-B709-11C112DD023F', CAST(0 AS bit))
,('E11A7CC4-D90F-43CB-80C3-0500A0BD75F8','2007-09-05 15:16:07.726','1992-02-25 23:43:45.169',4948,'veniam, quis nostrud exercita','consequat. Duis aute irure dolor in repreh',557,'https://picsum.photos/1200/900?image=1', '90C9479E-A11C-4D6D-AAAA-0405B6C0EFCD', CAST(0 AS bit))
;

GO

