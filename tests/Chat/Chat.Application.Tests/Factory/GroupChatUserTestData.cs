using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Tests.Factory;

internal static class GroupChatUserTestData
{
    public static GroupChatUser Create(
        string? id = null,
        string? username = null,
        int? chatId = null,
        UserId? appUserId = null,
        int? unreadMessages = null
    )
    {
        var entity = new GroupChatUser(
            id: id ?? "uid-1",
            username: username ?? "check",
            chatId: chatId ?? 1,
            appUserId: appUserId ?? "uid-1-1",
            unreadMessages: unreadMessages ?? 0
        );

        return entity;
    }

    public static GroupChatUserDto CreateDto(
        string? id = null,
        string? username = null,
        int? unreadMessages = null,
        int? lastReadMessageId = null,
        int? chatId = null,
        UserId? appUserId = null
    )
    {
        var entity = new GroupChatUserDto
        {
            Id = id ?? "uid-1",
            Username = username ?? "check",
            UnreadMessages = unreadMessages ?? 0,
            LastReadMessageId = lastReadMessageId ?? 1,
            GroupChatId = chatId ?? 1,
            AppUserId = appUserId ?? "uid-2"
        };

        return entity;
    }

    public static GroupChatUser[] CreateCollection(
        int size = 3
    )
    {
        var collection = new GroupChatUser[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new GroupChatUser(
                id: $"uid-{i}",
                username: $"check-{i}",
                chatId: 1 + i,
                appUserId: $"uid-1-{i}",
                unreadMessages: 0 + i
            );
        }

        return collection;
    }

    public static GroupChatUserDto[] CreateDtoCollection(
        int size = 3
    )
    {
        var collection = new GroupChatUserDto[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new GroupChatUserDto
            {
                Id = $"uid-{i}",
                Username = $"check-{i}",
                UnreadMessages = 0 + i,
                LastReadMessageId = null,
                GroupChatId = 1 + i,
                AppUserId = $"uid-1-{i}"
            };
        }

        return collection;
    }
}
