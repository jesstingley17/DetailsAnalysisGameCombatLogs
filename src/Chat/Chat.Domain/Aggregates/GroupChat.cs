using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.Enums.GroupChatRules;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class GroupChat
{
    private readonly List<GroupChatMessage> _messages = [];
    private readonly List<GroupChatUser> _users = [];

    private GroupChat() { }

    public GroupChat(string name, UserId ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }

        Name = name;
        OwnerId = ownerId;
    }

    public GroupChatId Id { get; }

    public string Name { get; }

    public UserId OwnerId { get; }

    public GroupChatRules? Rules { get; private set; }

    public IReadOnlyCollection<GroupChatMessage> Messages => _messages.AsReadOnly();

    public IReadOnlyCollection<GroupChatUser> Users => _users.AsReadOnly();

    public void AddRules(int chatId,
        InvitePeopleRestrictions invitePeople = InvitePeopleRestrictions.Anyone,
        RemovePeopleRestrictions removePeople = RemovePeopleRestrictions.Anyone,
        PinMessageRestrictions pinMessage = PinMessageRestrictions.Anyone,
        AnnouncementsRestrictions announcements = AnnouncementsRestrictions.Anyone)
    {
        Rules = new GroupChatRules(chatId, invitePeople, removePeople, pinMessage, announcements);
    }

    public void AddUser(string username, int chatId, UserId userId)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidOperationException("Username cannot be empty");
        }

        _users.Add(new GroupChatUser(Guid.NewGuid().ToString(), username, chatId, userId));
    }

    public void AddMessage(string username, string message, int chatId, GroupChatUserId senderId)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidOperationException("Username cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException("Message cannot be empty");
        }

        _messages.Add(new GroupChatMessage(username, message, chatId, senderId));
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

    public void MarkAsRead(string userId, int messageId)
    {
        var user = _users.Single(u => u.Id == userId);
        user.MarkAsRead(messageId);
    }

    public bool HasMessageBeenReadByAll(int messageId)
    {
        return _users.All(u => u.LastReadMessageId >= messageId);
    }
}
