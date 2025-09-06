IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AppUser] (
    [Id] nvarchar(450) NOT NULL,
    [Username] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [PhoneNumber] int NOT NULL,
    [Birthday] datetimeoffset NOT NULL,
    [AboutMe] nvarchar(max) NOT NULL,
    [Gender] int NOT NULL,
    [IdentityUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_AppUser] PRIMARY KEY ([Id])
);

CREATE TABLE [BannedUser] (
    [Id] int NOT NULL IDENTITY,
    [BannedCustomerId] nvarchar(max) NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_BannedUser] PRIMARY KEY ([Id])
);

CREATE TABLE [Customer] (
    [Id] nvarchar(450) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [PostalCode] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY ([Id])
);

CREATE TABLE [Friend] (
    [Id] int NOT NULL IDENTITY,
    [WhoFriendId] nvarchar(max) NOT NULL,
    [ForWhomId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Friend] PRIMARY KEY ([Id])
);

CREATE TABLE [RequestToConnet] (
    [Id] int NOT NULL IDENTITY,
    [ToAppUserId] nvarchar(max) NOT NULL,
    [When] datetimeoffset NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_RequestToConnet] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250122113825_InitialCreate', N'9.0.1');

COMMIT;
GO

