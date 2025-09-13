using Chat.Domain.Aggregates;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class GroupChatUser
{
    private GroupChatUser() { }

    public GroupChatUser(string id, string username, int chatId, UserId appUserId, int unreadMessages = 0)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty");
        }

        Id = id;
        Username = username;
        UnreadMessages = unreadMessages;
        GroupChatId = chatId;
        AppUserId = appUserId;
        LastReadMessageId = 0;
    }

    public GroupChatUserId Id { get; private set; }

    public string Username { get; private set; }

    public int UnreadMessages { get; private set; }

    public GroupChatMessageId LastReadMessageId { get; private set; }

    public GroupChatId GroupChatId { get; private set; }

    public UserId AppUserId { get; private set; }

    public GroupChat GroupChat { get; private set; } = null!;

    public void MarkAsRead(int messageId)
    {
        if (messageId > LastReadMessageId)
        {
            LastReadMessageId = messageId;
        }
    }
}
