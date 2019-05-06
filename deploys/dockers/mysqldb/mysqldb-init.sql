CREATE DATABASE maindb;
USE maindb;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `Carts` (
    `Id` char(36) NOT NULL,
    `Created` datetime(6) NOT NULL,
    `Updated` datetime(6) NOT NULL,
    `Version` int NOT NULL,
    `CartItemTotal` double NOT NULL,
    `CartItemPromoSavings` double NOT NULL,
    `ShippingTotal` double NOT NULL,
    `ShippingPromoSavings` double NOT NULL,
    `CartTotal` double NOT NULL,
    `IsCheckout` bit NOT NULL,
    CONSTRAINT `PK_Carts` PRIMARY KEY (`Id`)
);

CREATE TABLE `CartItems` (
    `Id` char(36) NOT NULL,
    `Created` datetime(6) NOT NULL,
    `Updated` datetime(6) NOT NULL,
    `Quantity` int NOT NULL,
    `Price` double NOT NULL,
    `PromoSavings` double NOT NULL,
    `CartId` char(36) NOT NULL,
    CONSTRAINT `PK_CartItems` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CartItems_Carts_CartId` FOREIGN KEY (`CartId`) REFERENCES `Carts` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `Products` (
    `Id` char(36) NOT NULL,
    CONSTRAINT `PK_Products` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Products_CartItems_Id` FOREIGN KEY (`Id`) REFERENCES `CartItems` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_CartItems_CartId` ON `CartItems` (`CartId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20190506031623_InitCartDb', '2.2.3-servicing-35854');

CREATE TABLE `Inventories` (
    `Id` char(36) NOT NULL,
    `Created` datetime(6) NOT NULL,
    `Updated` datetime(6) NOT NULL,
    `Version` int NOT NULL,
    `Location` longtext NULL,
    `Quantity` int NOT NULL,
    `Link` longtext NULL,
    CONSTRAINT `PK_Inventories` PRIMARY KEY (`Id`)
);

INSERT INTO `Inventories` (`Id`, `Created`, `Link`, `Location`, `Quantity`, `Updated`, `Version`)
VALUES ('25e6ba6e-fddb-401d-99b2-33ddc9f29322', '2019-04-07 07:47:46.581989', 'http://nashtechglobal.com', 'London, UK', 100, '0001-01-01 00:00:00.000000', 0);

INSERT INTO `Inventories` (`Id`, `Created`, `Link`, `Location`, `Quantity`, `Updated`, `Version`)
VALUES ('cab3818f-e459-412f-972f-d4b2d36aa735', '2019-04-07 07:47:46.582191', 'http://nashtechvietnam.com', 'Ho Chi Minh City, Vietnam', 1000, '0001-01-01 00:00:00.000000', 0);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20190407074747_InitInventoryDb', '2.2.3-servicing-35854');
