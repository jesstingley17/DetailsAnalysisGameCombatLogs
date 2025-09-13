using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class PersonalChatMessage
{
    private PersonalChatMessage() { }

    public PersonalChatMessage(string username, string message, PersonalChatId chatId, UserId appUserId,
                    MessageStatus status = MessageStatus.Sent,
                    MessageType type = MessageType.Default,
                    MessageMarkedType markedType = MessageMarkedType.None)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message cannot be empty");
        }

        Username = username;
        Message = message;
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

    public void EditMessage(string newMessage)
    {
        if (string.IsNullOrWhiteSpace(newMessage))
        {
            throw new ArgumentException("Message cannot be empty");
        }

        Message = newMessage;
        IsEdited = true;
        Time = DateTimeOffset.UtcNow;
    }

    public void UpdateStatus(MessageStatus newStatus)
    {
        Status = newStatus;
    }
}
