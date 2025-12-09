CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251025202602_InitialBaseline') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251025202602_InitialBaseline', '6.0.21');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251026195737_InitialCreate') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251026195737_InitialCreate') THEN

    CREATE TABLE `Users` (
        `UserId` int NOT NULL AUTO_INCREMENT,
        `FirstName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `LastName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Users` PRIMARY KEY (`UserId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251026195737_InitialCreate') THEN

    CREATE TABLE `Accounts` (
        `AccountId` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `AccountNumber` longtext CHARACTER SET utf8mb4 NOT NULL,
        `AccountType` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Accounts` PRIMARY KEY (`AccountId`),
        CONSTRAINT `FK_Accounts_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251026195737_InitialCreate') THEN

    CREATE TABLE `Transactions` (
        `TransactionId` int NOT NULL AUTO_INCREMENT,
        `AccountId` int NOT NULL,
        `Amount` decimal(65,30) NOT NULL,
        `Type` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Transactions` PRIMARY KEY (`TransactionId`),
        CONSTRAINT `FK_Transactions_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`AccountId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251026195737_InitialCreate') THEN

    CREATE INDEX `IX_Accounts_UserId` ON `Accounts` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251026195737_InitialCreate') THEN

    CREATE INDEX `IX_Transactions_AccountId` ON `Transactions` (`AccountId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251026195737_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_Users_Email` ON `Users` (`Email`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20251026195737_InitialCreate') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20251026195737_InitialCreate', '6.0.21');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

