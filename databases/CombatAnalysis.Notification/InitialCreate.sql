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
GO

CREATE TABLE [Notification] (
    [Id] int NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [Status] int NOT NULL,
    [InitiatorId] nvarchar(max) NOT NULL,
    [InitiatorName] nvarchar(max) NULL,
    [RecipientId] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Notification] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250623072614_InitialCreate-2', N'8.0.17');
GO

COMMIT;
GO

