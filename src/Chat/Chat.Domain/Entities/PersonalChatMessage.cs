using Chat.Domain.Aggregates;
using Chat.Domain.Enums;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class PersonalChatMessage : IRepositoryEntity<PersonalChatMessageId>
{
    private PersonalChatMessage() { }

    public PersonalChatMessage(int id, string username, string message, DateTimeOffset time, PersonalChatId chatId, UserId appUserId,
                MessageStatus status = MessageStatus.Sending,
                MessageType type = MessageType.Default,
                MessageMarkedType markedType = MessageMarkedType.None)
        : this(username, message, time, chatId, appUserId, status, type, markedType)
    {
        Id = id;
    }

    public PersonalChatMessage(string username, string message, DateTimeOffset time, PersonalChatId chatId, UserId appUserId,
                    MessageStatus status = MessageStatus.Sending,
                    MessageType type = MessageType.Default,
                    MessageMarkedType markedType = MessageMarkedType.None)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));

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

    public PersonalChatMessageId Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public DateTimeOffset Time { get; set; }

    public MessageStatus Status { get; set; }

    public MessageType Type { get; set; }

    public MessageMarkedType MarkedType { get; set; }

    public bool IsEdited { get; set; }

    public PersonalChatId PersonalChatId { get; set; }

    public UserId AppUserId { get; set; }

    public PersonalChat PersonalChat { get; private set; } = null!;

    public void ApplyUpdates(PersonalChatMessage updated)
    {
        EditMessage(updated.Message);
        UpdateStatus(updated.Status);
        UpdateMarker(updated.MarkedType);
    }

    public void EditMessage(string newMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newMessage, nameof(newMessage));

        if (!string.Equals(Message, newMessage, StringComparison.Ordinal))
        {
            Message = newMessage;
            IsEdited = true;
            Time = DateTimeOffset.UtcNow;
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
