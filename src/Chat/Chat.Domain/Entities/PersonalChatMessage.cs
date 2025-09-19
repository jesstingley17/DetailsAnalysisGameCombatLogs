using Chat.Domain.Aggregates;
using Chat.Domain.Enums;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class PersonalChatMessage : IRepositoryEntity<PersonalChatMessageId>
{
    public const int USERNAME_MAX_LENGTH = 64;
    public const int MESSAGE_MAX_LENGTH = 256;

    private PersonalChatMessage() { }

    public PersonalChatMessage(string username, string message, DateTimeOffset time, PersonalChatId chatId, UserId appUserId,
                    MessageStatus status = MessageStatus.Sending,
                    MessageType type = MessageType.Default,
                    MessageMarkedType markedType = MessageMarkedType.None)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(username.Length, USERNAME_MAX_LENGTH, nameof(username));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(message.Length, MESSAGE_MAX_LENGTH, nameof(message));

        Username = username;
        Message = message;
        Time = time;
        PersonalChatId = chatId;
        Status = status;
        Type = type;
        MarkedType = markedType;
        Time = DateTimeOffset.UtcNow;
        AppUserId = appUserId;
    }

    public PersonalChatMessageId Id { get; private set; }

    public string Username { get; private set; }

    public string Message { get; private set; }

    public DateTimeOffset Time { get; private set; }

    public MessageStatus Status { get; private set; }

    public MessageType Type { get; private set; }

    public MessageMarkedType MarkedType { get; private set; }

    public bool IsEdited { get; private set; }

    public PersonalChatId PersonalChatId { get; private set; }

    public UserId AppUserId { get; private set; }

    public PersonalChat PersonalChat { get; private set; } = null!;

    public void EditMessage(string newMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newMessage, nameof(newMessage));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newMessage.Length, MESSAGE_MAX_LENGTH, nameof(newMessage));

        if (!string.Equals(Message, newMessage, StringComparison.Ordinal))
        {
            Message = newMessage;
            IsEdited = true;
        }
    }

    public void UpdateStatus(MessageStatus newStatus)
    {
        if (!Status.Equals(newStatus))
        {
            Status = newStatus;
        }
    }

    public void UpdateMarker(MessageMarkedType newMarkedType)
    {
        if (!MarkedType.Equals(newMarkedType))
        {
            MarkedType = newMarkedType;
        }
    }
}
