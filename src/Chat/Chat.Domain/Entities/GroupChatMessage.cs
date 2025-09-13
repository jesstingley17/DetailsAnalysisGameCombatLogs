using Chat.Domain.Aggregates;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class GroupChatMessage
{
    private GroupChatMessage() { }

    public GroupChatMessage(string username, string message, int chatId, GroupChatUserId groupChatUserId,
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
        GroupChatId = chatId;
        GroupChatUserId = groupChatUserId;
        Status = status;
        Type = type;
        MarkedType = markedType;
        Time = DateTimeOffset.UtcNow;
    }

    public GroupChatMessageId Id { get; private set; }

    public string Username { get; private set; }

    public string Message { get; private set; }

    public DateTimeOffset Time { get; private set; }

    public MessageStatus Status { get; private set; }

    public MessageType Type { get; private set; }

    public MessageMarkedType MarkedType { get; private set; }

    public bool IsEdited { get; private set; }

    public GroupChatId GroupChatId { get; private set; }

    public GroupChatUserId GroupChatUserId { get; private set; }

    public GroupChat GroupChat { get; private set; } = null!;

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
