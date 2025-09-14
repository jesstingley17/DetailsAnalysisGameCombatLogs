using Chat.Domain.Aggregates;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class GroupChatUser : IRepositoryEntity<GroupChatUserId>
{
    private GroupChatUser() { }

    public GroupChatUser(string id, string username, int chatId, UserId appUserId, int unreadMessages = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));
        ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));

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

    public GroupChatMessageId? LastReadMessageId { get; private set; }

    public GroupChatId GroupChatId { get; private set; }

    public UserId AppUserId { get; private set; }

    public GroupChat GroupChat { get; private set; } = null!;

    public void ApplyUpdates(GroupChatUser updated)
    {
        ChangeGroupChatUsername(updated.Username);
        UpdateUnreadMessages(updated.UnreadMessages);

        if (updated.LastReadMessageId != null)
        {
            MarkAsRead(updated.LastReadMessageId);
        }
    }

    public void ChangeGroupChatUsername(string newUsername)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newUsername, nameof(newUsername));

        if (!string.Equals(Username, newUsername, StringComparison.Ordinal))
        {
            Username = newUsername;
        }
    }

    public void MarkAsRead(GroupChatMessageId? messageId)
    {
        ArgumentNullException.ThrowIfNull(messageId, nameof(messageId));

        if (LastReadMessageId == null
            || LastReadMessageId != null && !LastReadMessageId.Equals(messageId))
        {
            LastReadMessageId = messageId;
        }
    }

    public void UpdateUnreadMessages(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));

        if (UnreadMessages != count)
        {
            UnreadMessages = count;
        }
    }
}
