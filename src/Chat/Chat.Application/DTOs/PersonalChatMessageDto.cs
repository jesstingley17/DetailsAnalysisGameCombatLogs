using Chat.Domain.Enums;

namespace Chat.Application.DTOs;

public record PersonalChatMessageDto(int Id, string Username, string Message, DateTimeOffset Time, MessageStatus Status, MessageType Type, MessageMarkedType MarkedType, bool IsEdited, int PersonalChatId, string AppUserId);
