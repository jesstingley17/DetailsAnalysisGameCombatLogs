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

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251024133700_Init', N'9.0.10');

COMMIT;
GO

