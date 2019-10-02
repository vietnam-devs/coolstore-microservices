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
  [IsDeleted] bit NOT NULL,
	CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
) GO

INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl, IsDeleted) VALUES
('05233341-185A-468A-B074-00EBD08559AA','1991-08-02 15:33:00.040','2008-03-09 03:05:15.522',4612,'tempor incididunt ut labore et do','quis nostrud exercitation ull',638,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('3CB275C5-AA53-40FF-BC6A-015327053AF9','2016-05-10 05:01:44.437','2008-09-12 15:10:31.142',25414,'m','sin',671,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('A162B9EE-85B4-457A-93FC-015DF74201DD','1999-03-05 05:43:42.219','2006-10-27 13:42:49.099',10890,'ut labore et dolore magna aliqua. Ut enim ad minim ','dolor sit amet, consectetur adipiscing e',901,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('FF58A71D-76A2-41F8-AF44-018969694A59','1999-01-25 00:11:04.022','2006-12-03 03:01:37.382',17558,'non proident, sunt in culpa qui officia deserunt mollit anim id','est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididun',661,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('9032B448-61F2-45F8-9E95-020961441613','2005-05-17 08:39:00.444','2014-08-26 23:49:48.465',7106,'tempor incididunt ut labore ','ipsum dolor sit amet, consectetur adipis',80,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('D16E6353-0F88-43BA-9303-0241672D6AB6','2016-12-13 01:54:58.027','2017-01-08 15:29:22.082',12113,'aliqua. Ut enim ad minim veniam, quis nostrud exercitation ul','officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipi',275,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('80258882-2A90-4038-AC48-0283BB0AC9B7','2003-09-27 23:38:43.044','2010-09-18 14:00:34.626',1303,'velit esse cillum dolore eu fugiat ','mollit anim id est laborum.Lo',738,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('A11128B0-DD82-4179-99D9-0288E22DB70B','2011-03-19 15:43:25.445','1995-04-01 09:47:41.323',1182,'aute irure dolor in re','elit, sed do eiusmod tempor incididunt ut labore et dolore magna a',51,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('E96A0646-6508-4E40-A035-0294E3B6A017','2003-01-01 08:19:12.071','1980-12-03 15:01:03.026',26391,'cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsu','Excepteur sint occaecat cupidatat non proident, sunt in culpa qui ',847,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('D39650D3-7929-4702-BCB9-02978D2C2711','1989-07-28 11:42:10.666','1988-01-15 21:24:41.896',11689,'consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna a','voluptate velit esse cillum dolore eu fugiat nulla pariat',2,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
;
INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl, IsDeleted) VALUES
('E2207EA8-F965-41F9-ADC1-02B2DB6BCEDA','1980-02-20 19:44:23.924','1981-02-17 03:20:56.856',20155,'dolor in','irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Except',985,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('B5354991-897A-4B34-BC5D-02DE2DEF59A9','2011-08-21 09:27:19.085','1993-05-12 19:58:14.319',8202,'min','fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa ',768,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('88D41190-642A-4612-A010-030B2DBFD9C1','1984-09-29 12:43:40.750','1995-05-22 15:23:36.230',5446,'ut labore et dolore magna aliqua. Ut enim ad min','cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proid',504,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('46CF0EAA-E063-441C-8370-0378F75E2321','2009-06-22 03:58:18.671','2019-09-24 08:36:42.855',28565,'anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, se','dolore eu fugiat nulla par',432,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('9A1F6596-9B5A-4942-935C-039B37235DDD','1993-07-19 18:00:12.268','1988-08-26 10:59:03.959',8559,'aute irure dolor in reprehenderit in volupta','irure dolor in reprehenderit in voluptate veli',214,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('4FDF8E83-736D-493A-9500-03A80A08E086','1985-01-29 08:40:58.052','1993-09-06 02:01:38.318',15506,'Duis aute irure dolor in reprehenderit in voluptate veli','aliqua. Ut enim ad minim veniam, quis nos',684,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('F75197D1-2C04-4950-8E40-03D69B1DCB8C','1992-07-03 17:49:58.096','1998-02-09 13:10:05.477',32224,'ullamco laboris nisi ut aliqui','ex ea commodo consequat. Dui',637,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('B5BB05B9-98D9-416E-AC4E-040FDC7991A3','1990-01-19 01:22:33.299','2017-12-13 17:22:41.009',16645,'sint o','laboris nisi ut aliquip ex ea commodo conseq',351,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('CEF8483A-5546-41A5-87E8-04D0C05447A1','1994-05-12 21:36:07.716','2017-08-31 05:34:12.297',14785,'officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing eli','Excepteur sint occaecat cupidatat non pr',306,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('E11A7CC4-D90F-43CB-80C3-0500A0BD75F8','2007-09-05 15:16:07.726','1992-02-25 23:43:45.169',4948,'veniam, quis nostrud exercita','consequat. Duis aute irure dolor in repreh',557,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
;
INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl, IsDeleted) VALUES
('539EC431-750C-43FC-A8E1-052A1A3FDFA2','1980-12-16 15:53:26.841','2007-08-01 12:35:20.227',10103,'aute irure dolor in reprehenderit ','velit esse cillum dolore eu fugiat nulla p',571,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('F31A9005-0F3F-4DE2-8886-057A98F15ACA','2005-01-18 01:41:15.346','2000-01-24 10:01:27.670',29431,'m.Lorem ipsum do','laboris nisi ut aliquip ex ea commod',145,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('56E22662-4AEC-4F41-A38F-058534CC0FB3','1996-03-26 10:40:07.516','1991-01-17 16:59:41.809',3302,'officia deserun','commodo consequat. Duis aute irure dolor in reprehenderit',887,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('BE38C815-AF8F-465D-A79E-05895F9208B1','1985-11-29 17:44:34.434','2017-08-26 12:22:34.733',26614,'sit amet, consectetur adipiscing elit, sed do eiusmod t','ven',623,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('7DA5C29C-7484-4163-BF69-060738349BA1','1981-10-10 20:14:37.893','2016-02-28 16:56:58.155',30788,'voluptate velit esse cillum ','ex ea commodo consequat. Duis aute irure dolor in ',496,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('9E85467F-AFDC-4714-8BEC-06501B02B8B0','2005-02-24 05:58:53.823','2013-02-04 09:55:40.291',266,'elit, sed ','eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim ve',524,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('EABC1695-1148-4E54-8627-06735F306ED6','2006-10-10 00:27:46.612','1995-04-21 00:15:16.709',9895,'nulla pariatur. Excepteur sint occaecat cupidata','velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidat',881,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('780E4419-10A7-45AE-A60F-06B30126F86D','2010-12-08 23:21:13.238','2004-03-20 00:50:35.601',6615,'tempor incididunt ut labore et dolore magna aliqua. ','culpa qui officia deserunt mollit ani',358,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('06B6131D-E712-4914-B5E7-06D265420595','2014-08-25 21:46:11.088','1989-01-22 08:00:14.607',19258,'aliqua. Ut enim ad minim veniam, quis nostrud exercitatio','Excepteur sint occaecat cupidatat non pro',242,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
,('EC3020BD-6EDF-48C0-8D70-06F4656E97A7','2019-11-15 12:34:16.499','1990-02-05 09:45:54.652',30354,'elit, sed do eius','cillum dolore eu fugiat nulla pariatur. Excepteur sint occa',709,'https://picsum.photos/1200/900?image=1', CAST(0 AS bit))
;

GO

