using Chat.Domain.Enums;

namespace Chat.Domain.DTOs;

public record GroupChatMessageDto(
    int Id,
    string Username,
    string Message,
    DateTimeOffset Time,
    MessageStatus Status,
    MessageType Type,
    MessageMarkedType MarkedType,
    bool IsEdited,
    int GroupChatId,
    string GroupChatUserId,
    string AppUserId
    );
