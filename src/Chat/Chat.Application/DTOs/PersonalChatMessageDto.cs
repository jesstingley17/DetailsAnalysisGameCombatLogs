using Chat.Domain.Enums;

namespace Chat.Application.DTOs;

public class PersonalChatMessageDto
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public DateTimeOffset Time { get; set; }

    public MessageStatus Status { get; set; }

    public MessageType Type { get; set; }

    public MessageMarkedType MarkedType { get; set; }

    public int PersonalChatId { get; set; }

    public string AppUserId { get; set; }
}
