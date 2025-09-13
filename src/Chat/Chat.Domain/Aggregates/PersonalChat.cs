using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class PersonalChat
{
    private readonly List<PersonalChatMessage> _messages = [];

    private PersonalChat() { }

    public PersonalChat(UserId initiatorId, UserId companionId, int initiatorUnreadMessages = 0, int companionUnreadMessages = 0)
    {
        InitiatorId = initiatorId;
        CompanionId = companionId;
        InitiatorUnreadMessages = initiatorUnreadMessages;
        CompanionUnreadMessages = companionUnreadMessages;
    }

    public PersonalChatId Id { get; set; }

    public UserId InitiatorId { get; set; }

    public int InitiatorUnreadMessages { get; set; }

    public UserId CompanionId { get; set; }

    public int CompanionUnreadMessages { get; set; }

    public IReadOnlyCollection<PersonalChatMessage> Messages => _messages.AsReadOnly();

    public void AddMessage(string username, string message, int chatId, UserId senderId)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidOperationException("Username cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException("Message cannot be empty");
        }

        _messages.Add(new PersonalChatMessage(username, message, chatId, senderId));
    }

    public void EditMessage(int messageId, string newMessage)
    {
        var user = _messages.Single(u => u.Id == messageId);
        user.EditMessage(newMessage);
    }

    public void UpdateStatus(int messageId, MessageStatus newStatus)
    {
        var user = _messages.Single(u => u.Id == messageId);
        user.UpdateStatus(newStatus);
    }
}
