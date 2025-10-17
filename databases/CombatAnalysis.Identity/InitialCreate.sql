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
CREATE TABLE [AuthorizationCodeChallenge] (
    [Id] nvarchar(450) NOT NULL,
    [CodeChallenge] nvarchar(max) NOT NULL,
    [CodeChallengeMethod] nvarchar(max) NOT NULL,
    [RedirectUrl] nvarchar(max) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [ExpiryTime] datetimeoffset NOT NULL,
    [ClientId] nvarchar(max) NOT NULL,
    [IsUsed] bit NOT NULL,
    CONSTRAINT [PK_AuthorizationCodeChallenge] PRIMARY KEY ([Id])
);

CREATE TABLE [Client] (
    [Id] nvarchar(450) NOT NULL,
    [RedirectUrl] nvarchar(max) NOT NULL,
    [AllowedScopes] nvarchar(max) NOT NULL,
    [AllowedAudiences] nvarchar(max) NOT NULL,
    [ClientName] nvarchar(max) NOT NULL,
    [ClientType] int NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [UpdatedAt] datetimeoffset NOT NULL,
    [ClientSecret] nvarchar(max) NULL,
    CONSTRAINT [PK_Client] PRIMARY KEY ([Id])
);

CREATE TABLE [IdentityUser] (
    [Id] nvarchar(450) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Salt] nvarchar(max) NOT NULL,
    [EmailVerified] bit NOT NULL,
    CONSTRAINT [PK_IdentityUser] PRIMARY KEY ([Id])
);

CREATE TABLE [RefreshToken] (
    [Id] nvarchar(450) NOT NULL,
    [TokenHash] nvarchar(max) NOT NULL,
    [TokenSalt] nvarchar(max) NOT NULL,
    [CreatedAt] datetimeoffset NOT NULL,
    [ExpiresAt] datetimeoffset NOT NULL,
    [RevokedAt] datetimeoffset NULL,
    [ClientId] nvarchar(max) NOT NULL,
    [UserId] nvarchar(max) NOT NULL,
    [ReplacedByTokenId] nvarchar(max) NULL,
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY ([Id])
);

CREATE TABLE [ResetToken] (
    [Id] int NOT NULL IDENTITY,
    [Email] nvarchar(max) NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [ExpirationTime] datetime2 NOT NULL,
    [IsUsed] bit NOT NULL,
    CONSTRAINT [PK_ResetToken] PRIMARY KEY ([Id])
);

CREATE TABLE [VerifyEmailToken] (
    [Id] int NOT NULL IDENTITY,
    [Email] nvarchar(max) NOT NULL,
    [Token] nvarchar(max) NOT NULL,
    [ExpirationTime] datetime2 NOT NULL,
    [IsUsed] bit NOT NULL,
    CONSTRAINT [PK_VerifyEmailToken] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AllowedAudiences', N'AllowedScopes', N'ClientName', N'ClientSecret', N'ClientType', N'CreatedAt', N'IsActive', N'RedirectUrl', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Client]'))
    SET IDENTITY_INSERT [Client] ON;
INSERT INTO [Client] ([Id], [AllowedAudiences], [AllowedScopes], [ClientName], [ClientSecret], [ClientType], [CreatedAt], [IsActive], [RedirectUrl], [UpdatedAt])
VALUES (N'18a67288-d050-4fa3-887e-9551dc5d2d85', N'user-api,chat-api,communication-api,hubs,notification-api', N'api.read,api.write', N'web', NULL, 0, '2025-10-17T15:47:39.5860396+03:00', CAST(1 AS bit), N'localhost:5173/callback', '2025-10-17T15:47:39.5860399+03:00'),
(N'c3785216-d952-4cf6-b0f6-ad8f142fe28d', N'user-api,chat-api,communication-api,hubs,notification-api', N'api.read,api.write', N'desktop', NULL, 0, '2025-10-17T15:47:39.5846410+03:00', CAST(1 AS bit), N'localhost:45571/callback', '2025-10-17T15:47:39.5860165+03:00');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AllowedAudiences', N'AllowedScopes', N'ClientName', N'ClientSecret', N'ClientType', N'CreatedAt', N'IsActive', N'RedirectUrl', N'UpdatedAt') AND [object_id] = OBJECT_ID(N'[Client]'))
    SET IDENTITY_INSERT [Client] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251017124739_Prod', N'9.0.1');

COMMIT;
GO

