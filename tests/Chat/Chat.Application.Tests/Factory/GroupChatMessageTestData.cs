using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Tests.Factory;

internal static class GroupChatMessageTestData
{
    public static GroupChatMessage Create(
        string? username = null,
        string? message = null,
        int? chatId = null,
        GroupChatUserId? groupChatUserId = null,
        MessageStatus? status = null,
        MessageType? type = null,
        MessageMarkedType? markedType = null
    )
    {
        var entity = new GroupChatMessage(
            username: username ?? "chat-1",
            message: message ?? "test message",
            chatId: chatId ?? 1,
            groupChatUserId: groupChatUserId ?? "uid-1",
            status: status ?? MessageStatus.Sent,
            type: type ?? MessageType.Default,
            markedType: markedType ?? MessageMarkedType.None
        );

        return entity;
    }

    public static GroupChatMessageDto CreateDto(
        int? id = null,
        string? username = null,
        string? message = null,
        int? chatId = null,
        GroupChatUserId? groupChatUserId = null,
        MessageStatus? status = null,
        MessageType? type = null,
        MessageMarkedType? markedType = null
    )
    {
        var entity = new GroupChatMessageDto
        {
            Id = id ?? 1,
            Username = username ?? "check",
            Message = message ?? "test message",
            GroupChatId = chatId ?? 1,
            GroupChatUserId = groupChatUserId ?? "uid-1",
            Status = status ?? MessageStatus.Sent,
            Type = type ?? MessageType.Default,
            MarkedType = markedType ?? MessageMarkedType.None
        };

        return entity;
    }

    public static GroupChatMessage[] CreateCollection(
        int size = 3
    )
    {
        var collection = new GroupChatMessage[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new GroupChatMessage(
                username: $"chat-{i}",
                message: $"test message {i}",
                chatId: i + 1,
                groupChatUserId: $"uid-{i}",
                status: MessageStatus.Sent,
                type: MessageType.Default,
                markedType: MessageMarkedType.None
            );
        }

        return collection;
    }

    public static GroupChatMessageDto[] CreateDtoCollection(
        int size = 3
    )
    {
        var collection = new GroupChatMessageDto[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new GroupChatMessageDto
            {
                Id = 1 + i,
                Username = $"check-{i}",
                Message = $"test message {i}",
                GroupChatId = 1 + i,
                GroupChatUserId = $"uid-1-{i}",
                Status = MessageStatus.Sent,
                Type = MessageType.Default,
                MarkedType = MessageMarkedType.None
            };
        }

        return collection;
    }
}
