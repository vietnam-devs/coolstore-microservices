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


