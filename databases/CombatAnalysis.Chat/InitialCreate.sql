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
    [Name] nvarchar(max) NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChat] PRIMARY KEY ([Id])
);

CREATE TABLE [GroupChatMessage] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [Time] datetimeoffset NOT NULL,
    [Status] int NOT NULL,
    [Type] int NOT NULL,
    [MarkedType] int NOT NULL,
    [IsEdited] bit NOT NULL,
    [ChatId] int NOT NULL,
    [GroupChatUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChatMessage] PRIMARY KEY ([Id])
);

CREATE TABLE [GroupChatRules] (
    [Id] int NOT NULL IDENTITY,
    [InvitePeople] int NOT NULL,
    [RemovePeople] int NOT NULL,
    [PinMessage] int NOT NULL,
    [Announcements] int NOT NULL,
    [ChatId] int NOT NULL,
    CONSTRAINT [PK_GroupChatRules] PRIMARY KEY ([Id])
);

CREATE TABLE [GroupChatUser] (
    [Id] nvarchar(450) NOT NULL,
    [Username] nvarchar(max) NOT NULL,
    [UnreadMessages] int NOT NULL,
    [ChatId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_GroupChatUser] PRIMARY KEY ([Id])
);

CREATE TABLE [PersonalChat] (
    [Id] int NOT NULL IDENTITY,
    [InitiatorId] nvarchar(max) NOT NULL,
    [InitiatorUnreadMessages] int NOT NULL,
    [CompanionId] nvarchar(max) NOT NULL,
    [CompanionUnreadMessages] int NOT NULL,
    CONSTRAINT [PK_PersonalChat] PRIMARY KEY ([Id])
);

CREATE TABLE [PersonalChatMessage] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [Time] datetimeoffset NOT NULL,
    [Status] int NOT NULL,
    [Type] int NOT NULL,
    [MarkedType] int NOT NULL,
    [IsEdited] bit NOT NULL,
    [ChatId] int NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PersonalChatMessage] PRIMARY KEY ([Id])
);

CREATE TABLE [UnreadGroupChatMessage] (
    [Id] int NOT NULL IDENTITY,
    [GroupChatUserId] nvarchar(max) NOT NULL,
    [GroupChatMessageId] int NOT NULL,
    CONSTRAINT [PK_UnreadGroupChatMessage] PRIMARY KEY ([Id])
);

CREATE TABLE [VoiceChat] (
    [Id] nvarchar(450) NOT NULL,
    [AppUserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_VoiceChat] PRIMARY KEY ([Id])
);
GO

CREATE PROCEDURE GetPersonalChatMessageByChatIdPagination (@chatId INT, @pageSize INT)
AS
BEGIN
	SELECT TOP (@pageSize) * 
	FROM PersonalChatMessage
	WHERE ChatId = @chatId
	ORDER BY Id DESC
END
GO

CREATE PROCEDURE GetPersonalChatMessageByChatIdMore (@chatId INT, @offset INT, @pageSize INT)
AS
BEGIN
	SELECT * 
	FROM PersonalChatMessage
	WHERE ChatId = @chatId
	ORDER BY Id DESC
	OFFSET @offset ROWS
	FETCH NEXT @pageSize ROWS ONLY
END
GO

CREATE PROCEDURE GetGroupChatMessageByChatIdPagination (@chatId INT, @groupChatUserId NVARCHAR (MAX), @pageSize INT)
AS
BEGIN
    SELECT TOP (@pageSize)
        gcm.*,
        (
		    SELECT TOP 1 GroupChatMessageId
            FROM UnreadGroupChatMessage ugcm
            WHERE (ugcm.GroupChatMessageId = gcm.Id AND ugcm.GroupChatUserId = @groupChatUserId)
			    OR (ugcm.GroupChatMessageId = gcm.Id AND gcm.GroupChatUserId = @groupChatUserId)
        ) AS GroupChatMessageId
    FROM GroupChatMessage gcm
    WHERE gcm.ChatId = @chatId
    ORDER BY gcm.Id DESC
END
GO

CREATE PROCEDURE GetGroupChatMessageByChatIdMore (@chatId INT, @groupChatUserId NVARCHAR (MAX), @offset INT, @pageSize INT)
AS
BEGIN
    SELECT
        gcm.*,
        (
		    SELECT TOP 1 GroupChatMessageId
            FROM UnreadGroupChatMessage ugcm
            WHERE (ugcm.GroupChatMessageId = gcm.Id AND ugcm.GroupChatUserId = @groupChatUserId)
			    OR (ugcm.GroupChatMessageId = gcm.Id AND gcm.GroupChatUserId = @groupChatUserId)
        ) AS GroupChatMessageId
    FROM GroupChatMessage gcm
    WHERE gcm.ChatId = @chatId
    ORDER BY gcm.Id DESC
    OFFSET @offset ROWS
    FETCH NEXT @pageSize ROWS ONLY
END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250627164146_InitialCreate', N'9.0.1');

COMMIT;
GO

