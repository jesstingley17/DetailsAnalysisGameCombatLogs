using Chat.Domain.Entities;
using Chat.Domain.Enums.GroupChatRules;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class GroupChat : IRepositoryEntity<GroupChatId>
{
    private readonly List<GroupChatMessage> _messages = [];
    private readonly List<GroupChatUser> _users = [];

    public const int MaxNameLength = 128;

    private GroupChat() { }

    public GroupChat(int id, string name, UserId ownerId) : this(name, ownerId)
    {
        Id = id;
    }

    public GroupChat(string name, UserId ownerId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, MaxNameLength, nameof(name));

        Name = name;
        OwnerId = ownerId;
    }

    public GroupChatId Id { get; private set; }

    public string Name { get; private set; }

    public UserId OwnerId { get; private set; }

    public GroupChatRules? Rules { get; private set; }

    public IReadOnlyCollection<GroupChatMessage> Messages => _messages.AsReadOnly();

    public IReadOnlyCollection<GroupChatUser> Users => _users.AsReadOnly();

    public void ApplyUpdates(GroupChat updated)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(updated.Name, nameof(updated.Name));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(updated.Name.Length, MaxNameLength, nameof(updated.Name));

        if (!string.Equals(Name, updated.Name, StringComparison.Ordinal))
        {
            Name = updated.Name;
        }

        if (!OwnerId.Equals(updated.OwnerId))
        {
            OwnerId = updated.OwnerId;
        }
    }

    public void AddRules(int chatId,
        InvitePeopleRestrictions invitePeople = InvitePeopleRestrictions.Anyone,
        RemovePeopleRestrictions removePeople = RemovePeopleRestrictions.Anyone,
        PinMessageRestrictions pinMessage = PinMessageRestrictions.Anyone,
        AnnouncementsRestrictions announcements = AnnouncementsRestrictions.Anyone)
    {
        Rules = new GroupChatRules(chatId, invitePeople, removePeople, pinMessage, announcements);
    }

    public void RemoveRules()
    {
        Rules = null;
    }

    public void UpdateRules(GroupChatRules rules)
    {
        ArgumentNullException.ThrowIfNull(Rules, nameof(Rules));

        Rules.Update(rules.InvitePeople, rules.RemovePeople, rules.PinMessage, rules.Announcements);
    }

    public void EnsureUserIsMember(GroupChatUser user)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(user.GroupChatId, Id, nameof(user));
    }
}
