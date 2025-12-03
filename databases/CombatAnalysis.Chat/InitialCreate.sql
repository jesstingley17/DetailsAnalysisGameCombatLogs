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
CREATE TABLE [GroupChat] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [OwnerId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChat] PRIMARY KEY ([Id])
);

CREATE TABLE [PersonalChat] (
    [Id] int NOT NULL IDENTITY,
    [InitiatorId] nvarchar(max) NOT NULL,
    [InitiatorUnreadMessages] int NOT NULL,
    [CompanionId] nvarchar(max) NOT NULL,
    [CompanionUnreadMessages] int NOT NULL,
    CONSTRAINT [PK_PersonalChat] PRIMARY KEY ([Id])
);

CREATE TABLE [VoiceChat] (
    [Id] nvarchar(450) NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_VoiceChat] PRIMARY KEY ([Id])
);

CREATE TABLE [GroupChatMessage] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(64) NOT NULL,
    [Message] nvarchar(256) NOT NULL,
    [Time] datetimeoffset NOT NULL,
    [Status] int NOT NULL,
    [Type] int NOT NULL,
    [MarkedType] int NOT NULL,
    [IsEdited] bit NOT NULL,
    [GroupChatId] int NOT NULL,
    [GroupChatUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChatMessage] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GroupChatMessage_GroupChat_GroupChatId] FOREIGN KEY ([GroupChatId]) REFERENCES [GroupChat] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [GroupChatRules] (
    [Id] int NOT NULL IDENTITY,
    [InvitePeople] int NOT NULL,
    [RemovePeople] int NOT NULL,
    [PinMessage] int NOT NULL,
    [Announcements] int NOT NULL,
    [GroupChatId] int NOT NULL,
    CONSTRAINT [PK_GroupChatRules] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GroupChatRules_GroupChat_GroupChatId] FOREIGN KEY ([GroupChatId]) REFERENCES [GroupChat] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [GroupChatUser] (
    [Id] nvarchar(450) NOT NULL,
    [Username] nvarchar(64) NOT NULL,
    [UnreadMessages] int NOT NULL,
    [LastReadMessageId] int NULL,
    [GroupChatId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChatUser] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GroupChatUser_GroupChat_GroupChatId] FOREIGN KEY ([GroupChatId]) REFERENCES [GroupChat] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [PersonalChatMessage] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(64) NOT NULL,
    [Message] nvarchar(256) NOT NULL,
    [Time] datetimeoffset NOT NULL,
    [Status] int NOT NULL,
    [Type] int NOT NULL,
    [MarkedType] int NOT NULL,
    [IsEdited] bit NOT NULL,
    [PersonalChatId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PersonalChatMessage] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PersonalChatMessage_PersonalChat_PersonalChatId] FOREIGN KEY ([PersonalChatId]) REFERENCES [PersonalChat] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_GroupChatMessage_GroupChatId] ON [GroupChatMessage] ([GroupChatId]);

CREATE UNIQUE INDEX [IX_GroupChatRules_GroupChatId] ON [GroupChatRules] ([GroupChatId]);

CREATE INDEX [IX_GroupChatUser_GroupChatId] ON [GroupChatUser] ([GroupChatId]);

CREATE INDEX [IX_PersonalChatMessage_PersonalChatId] ON [PersonalChatMessage] ([PersonalChatId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251004104606_Prod', N'9.0.9');

COMMIT;
GO

