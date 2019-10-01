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
	CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
) GO

INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl) VALUES 
('05233341-185A-468A-B074-00EBD08559AA','1991-08-02 15:33:00.040','2008-03-09 03:05:15.522',4612,'tempor incididunt ut labore et do','quis nostrud exercitation ull',638,'https://picsum.photos/1200/900?image=1')
,('3CB275C5-AA53-40FF-BC6A-015327053AF9','2016-05-10 05:01:44.437','2008-09-12 15:10:31.142',25414,'m','sin',671,'https://picsum.photos/1200/900?image=1')
,('A162B9EE-85B4-457A-93FC-015DF74201DD','1999-03-05 05:43:42.219','2006-10-27 13:42:49.099',10890,'ut labore et dolore magna aliqua. Ut enim ad minim ','dolor sit amet, consectetur adipiscing e',901,'https://picsum.photos/1200/900?image=1')
,('FF58A71D-76A2-41F8-AF44-018969694A59','1999-01-25 00:11:04.022','2006-12-03 03:01:37.382',17558,'non proident, sunt in culpa qui officia deserunt mollit anim id','est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididun',661,'https://picsum.photos/1200/900?image=1')
,('9032B448-61F2-45F8-9E95-020961441613','2005-05-17 08:39:00.444','2014-08-26 23:49:48.465',7106,'tempor incididunt ut labore ','ipsum dolor sit amet, consectetur adipis',80,'https://picsum.photos/1200/900?image=1')
,('D16E6353-0F88-43BA-9303-0241672D6AB6','2016-12-13 01:54:58.027','2017-01-08 15:29:22.082',12113,'aliqua. Ut enim ad minim veniam, quis nostrud exercitation ul','officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipi',275,'https://picsum.photos/1200/900?image=1')
,('80258882-2A90-4038-AC48-0283BB0AC9B7','2003-09-27 23:38:43.044','2010-09-18 14:00:34.626',1303,'velit esse cillum dolore eu fugiat ','mollit anim id est laborum.Lo',738,'https://picsum.photos/1200/900?image=1')
,('A11128B0-DD82-4179-99D9-0288E22DB70B','2011-03-19 15:43:25.445','1995-04-01 09:47:41.323',1182,'aute irure dolor in re','elit, sed do eiusmod tempor incididunt ut labore et dolore magna a',51,'https://picsum.photos/1200/900?image=1')
,('E96A0646-6508-4E40-A035-0294E3B6A017','2003-01-01 08:19:12.071','1980-12-03 15:01:03.026',26391,'cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsu','Excepteur sint occaecat cupidatat non proident, sunt in culpa qui ',847,'https://picsum.photos/1200/900?image=1')
,('D39650D3-7929-4702-BCB9-02978D2C2711','1989-07-28 11:42:10.666','1988-01-15 21:24:41.896',11689,'consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna a','voluptate velit esse cillum dolore eu fugiat nulla pariat',2,'https://picsum.photos/1200/900?image=1')
;
INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl) VALUES 
('E2207EA8-F965-41F9-ADC1-02B2DB6BCEDA','1980-02-20 19:44:23.924','1981-02-17 03:20:56.856',20155,'dolor in','irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Except',985,'https://picsum.photos/1200/900?image=1')
,('B5354991-897A-4B34-BC5D-02DE2DEF59A9','2011-08-21 09:27:19.085','1993-05-12 19:58:14.319',8202,'min','fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa ',768,'https://picsum.photos/1200/900?image=1')
,('88D41190-642A-4612-A010-030B2DBFD9C1','1984-09-29 12:43:40.750','1995-05-22 15:23:36.230',5446,'ut labore et dolore magna aliqua. Ut enim ad min','cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proid',504,'https://picsum.photos/1200/900?image=1')
,('46CF0EAA-E063-441C-8370-0378F75E2321','2009-06-22 03:58:18.671','2019-09-24 08:36:42.855',28565,'anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, se','dolore eu fugiat nulla par',432,'https://picsum.photos/1200/900?image=1')
,('9A1F6596-9B5A-4942-935C-039B37235DDD','1993-07-19 18:00:12.268','1988-08-26 10:59:03.959',8559,'aute irure dolor in reprehenderit in volupta','irure dolor in reprehenderit in voluptate veli',214,'https://picsum.photos/1200/900?image=1')
,('4FDF8E83-736D-493A-9500-03A80A08E086','1985-01-29 08:40:58.052','1993-09-06 02:01:38.318',15506,'Duis aute irure dolor in reprehenderit in voluptate veli','aliqua. Ut enim ad minim veniam, quis nos',684,'https://picsum.photos/1200/900?image=1')
,('F75197D1-2C04-4950-8E40-03D69B1DCB8C','1992-07-03 17:49:58.096','1998-02-09 13:10:05.477',32224,'ullamco laboris nisi ut aliqui','ex ea commodo consequat. Dui',637,'https://picsum.photos/1200/900?image=1')
,('B5BB05B9-98D9-416E-AC4E-040FDC7991A3','1990-01-19 01:22:33.299','2017-12-13 17:22:41.009',16645,'sint o','laboris nisi ut aliquip ex ea commodo conseq',351,'https://picsum.photos/1200/900?image=1')
,('CEF8483A-5546-41A5-87E8-04D0C05447A1','1994-05-12 21:36:07.716','2017-08-31 05:34:12.297',14785,'officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing eli','Excepteur sint occaecat cupidatat non pr',306,'https://picsum.photos/1200/900?image=1')
,('E11A7CC4-D90F-43CB-80C3-0500A0BD75F8','2007-09-05 15:16:07.726','1992-02-25 23:43:45.169',4948,'veniam, quis nostrud exercita','consequat. Duis aute irure dolor in repreh',557,'https://picsum.photos/1200/900?image=1')
;
INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl) VALUES 
('539EC431-750C-43FC-A8E1-052A1A3FDFA2','1980-12-16 15:53:26.841','2007-08-01 12:35:20.227',10103,'aute irure dolor in reprehenderit ','velit esse cillum dolore eu fugiat nulla p',571,'https://picsum.photos/1200/900?image=1')
,('F31A9005-0F3F-4DE2-8886-057A98F15ACA','2005-01-18 01:41:15.346','2000-01-24 10:01:27.670',29431,'m.Lorem ipsum do','laboris nisi ut aliquip ex ea commod',145,'https://picsum.photos/1200/900?image=1')
,('56E22662-4AEC-4F41-A38F-058534CC0FB3','1996-03-26 10:40:07.516','1991-01-17 16:59:41.809',3302,'officia deserun','commodo consequat. Duis aute irure dolor in reprehenderit',887,'https://picsum.photos/1200/900?image=1')
,('BE38C815-AF8F-465D-A79E-05895F9208B1','1985-11-29 17:44:34.434','2017-08-26 12:22:34.733',26614,'sit amet, consectetur adipiscing elit, sed do eiusmod t','ven',623,'https://picsum.photos/1200/900?image=1')
,('7DA5C29C-7484-4163-BF69-060738349BA1','1981-10-10 20:14:37.893','2016-02-28 16:56:58.155',30788,'voluptate velit esse cillum ','ex ea commodo consequat. Duis aute irure dolor in ',496,'https://picsum.photos/1200/900?image=1')
,('9E85467F-AFDC-4714-8BEC-06501B02B8B0','2005-02-24 05:58:53.823','2013-02-04 09:55:40.291',266,'elit, sed ','eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim ve',524,'https://picsum.photos/1200/900?image=1')
,('EABC1695-1148-4E54-8627-06735F306ED6','2006-10-10 00:27:46.612','1995-04-21 00:15:16.709',9895,'nulla pariatur. Excepteur sint occaecat cupidata','velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidat',881,'https://picsum.photos/1200/900?image=1')
,('780E4419-10A7-45AE-A60F-06B30126F86D','2010-12-08 23:21:13.238','2004-03-20 00:50:35.601',6615,'tempor incididunt ut labore et dolore magna aliqua. ','culpa qui officia deserunt mollit ani',358,'https://picsum.photos/1200/900?image=1')
,('06B6131D-E712-4914-B5E7-06D265420595','2014-08-25 21:46:11.088','1989-01-22 08:00:14.607',19258,'aliqua. Ut enim ad minim veniam, quis nostrud exercitatio','Excepteur sint occaecat cupidatat non pro',242,'https://picsum.photos/1200/900?image=1')
,('EC3020BD-6EDF-48C0-8D70-06F4656E97A7','2019-11-15 12:34:16.499','1990-02-05 09:45:54.652',30354,'elit, sed do eius','cillum dolore eu fugiat nulla pariatur. Excepteur sint occa',709,'https://picsum.photos/1200/900?image=1')
;
INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl) VALUES 
('3175F15C-9003-4FBC-B43D-072DB179DCBF','1981-05-14 10:44:47.303','2003-01-22 15:17:02.107',638,'Excepteur sint occaecat cupidatat non proiden','in reprehenderit in volupt',800,'https://picsum.photos/1200/900?image=1')
,('64FB1765-4C1A-4FDB-80A4-07928E8CB425','2007-01-26 06:15:09.084','1988-01-06 07:47:22.933',7386,'tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nost','Excepteur sint occaecat cupidatat non p',289,'https://picsum.photos/1200/900?image=1')
,('5A85912E-C5C4-43AE-823D-079FB90BB313','2002-07-04 22:05:47.608','1995-08-26 13:53:19.491',11405,'in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur s','dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in c',889,'https://picsum.photos/1200/900?image=1')
,('29BFF794-5EF4-4B72-B3BA-07C4448924D0','2013-01-27 10:29:51.883','2005-03-03 21:42:48.287',25691,'incididunt ut labo','voluptate velit esse cillum dolore eu fugiat nulla pariatur. Except',237,'https://picsum.photos/1200/900?image=1')
,('DB3AD298-7CF8-4E4B-9227-0830BE182DD8','2000-04-23 06:38:38.965','2016-07-26 20:26:29.569',11620,'tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud','aliqui',999,'https://picsum.photos/1200/900?image=1')
,('D68E8ACA-95FD-446A-BEBB-085C3AE9A896','2016-08-18 03:10:29.463','1997-05-05 01:46:53.626',18986,'occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit','deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet,',161,'https://picsum.photos/1200/900?image=1')
,('A319B71B-0C0B-4900-A2E9-0888256F863E','2000-01-29 20:08:18.698','1984-05-06 22:48:20.786',20130,'occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.','Duis aute irure dolor in reprehenderit i',118,'https://picsum.photos/1200/900?image=1')
,('0B24C107-6C5C-4C0C-A3E6-08899D1BA616','1996-06-12 19:59:31.430','2011-03-03 19:27:54.701',11772,'qui o','ut labore et dolore magna aliqua. Ut enim ad m',16,'https://picsum.photos/1200/900?image=1')
,('54194500-8476-4785-8BA2-08B1FEA2358B','1984-01-04 23:25:37.813','1989-11-11 03:59:57.938',11738,'exercitation ullamco laboris nisi ut aliqu','sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est la',970,'https://picsum.photos/1200/900?image=1')
,('F7215532-C765-4989-B383-092DA5A49751','2014-04-13 22:07:33.997','2019-01-01 05:36:32.655',23796,'la','nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehender',297,'https://picsum.photos/1200/900?image=1')
;
INSERT INTO ProductCatalogDb.[catalog].Products (Id,Created,Updated,Version,Name,Description,Price,ImageUrl) VALUES 
('B8A9A65B-039B-4BA6-83DF-09538876D697','2001-11-08 11:17:05.026','2000-07-23 02:41:59.247',5855,'sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut l','Ut eni',87,'https://picsum.photos/1200/900?image=1')
,('9E07C29A-3B34-4EBF-BE82-09B0E7254CAF','2001-12-09 02:19:59.126','1986-05-23 23:21:58.867',27452,'sed do eiusmod tempor incididunt ut labor','ali',801,'https://picsum.photos/1200/900?image=1')
,('F75468D2-E6A8-487D-A56F-09F30B06516B','1999-01-12 20:32:40.349','2004-01-25 11:05:27.683',29542,'ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate ','esse cillum dolore',882,'https://picsum.photos/1200/900?image=1')
,('CC505F63-ECEA-4562-873D-0A0EB64A48B7','2007-04-28 04:14:51.796','1997-07-18 19:15:16.365',5162,'exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Du','Duis aute irure dolor in reprehe',756,'https://picsum.photos/1200/900?image=1')
,('8E4466AC-0162-4808-8781-0A5F93AA5E2D','2019-10-03 13:22:38.502','1980-01-23 03:29:27.244',26537,'nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt m','anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod t',915,'https://picsum.photos/1200/900?image=1')
,('0D1AD8C2-9E90-4836-B3D4-0A828270E44D','1983-06-28 04:22:40.208','2012-03-12 18:41:57.106',15771,'nisi ut aliquip ex ea commodo consequa','non proident, sunt in culpa qui officia deserunt mollit anim id est',777,'https://picsum.photos/1200/900?image=1')
,('F09E86DD-B95B-4F28-AC4A-0A8BA9FA2D84','2000-09-02 21:03:26.223','2004-09-04 20:38:58.295',7637,'esse cillum dolore eu fugiat nulla pariatur. E','elit, sed do eiusmod tempor incididunt ut labore et dolo',135,'https://picsum.photos/1200/900?image=1')
,('3825D6B3-9EF0-431E-8605-0AAFAF9807B3','2016-02-21 06:55:08.013','1992-03-22 12:31:23.228',15640,'incididunt ut labore et dolore magna aliqua. Ut enim ','laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in repr',143,'https://picsum.photos/1200/900?image=1')
,('BA1E4DEE-AE00-403D-BE17-0AB06F25944D','2016-08-28 20:30:00.374','1997-06-17 07:07:21.147',7884,'Duis aute irure dolor in re','aliq',5,'https://picsum.photos/1200/900?image=1')
,('A239ACAA-507D-4A3A-B4B3-0AB363909EBB','1998-06-10 17:12:29.744','1996-03-18 14:53:16.156',31822,'in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Exc','cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id',929,'https://picsum.photos/1200/900?image=1')
;

GO

