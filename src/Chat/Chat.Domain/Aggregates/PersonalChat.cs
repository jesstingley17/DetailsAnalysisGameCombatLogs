using Chat.Domain.Entities;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class PersonalChat : IRepositoryEntity<PersonalChatId>
{
    private readonly List<PersonalChatMessage> _messages = [];

    private PersonalChat() { }

    public PersonalChat(int id, UserId initiatorId, UserId companionId, int initiatorUnreadMessages = 0, int companionUnreadMessages = 0) 
        : this(initiatorId, companionId, initiatorUnreadMessages, companionUnreadMessages)
    {
        Id = id;
    }

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

    public void UpdateInitiatorUnreadMessageCount(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));

        if (InitiatorUnreadMessages != count)
        {
            InitiatorUnreadMessages = count;
        }
    }
    public void UpdateCompanionUnreadMessageCount(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));

        if (CompanionUnreadMessages != count)
        {
            CompanionUnreadMessages = count;
        }
    }

}
