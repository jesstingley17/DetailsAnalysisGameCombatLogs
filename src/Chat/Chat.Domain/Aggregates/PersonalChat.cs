using Chat.Domain.Entities;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class PersonalChat : IRepositoryEntity<PersonalChatId>
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

    public PersonalChatId Id { get; private set; }

    public UserId InitiatorId { get; private set; }

    public int InitiatorUnreadMessages { get; private set; }

    public UserId CompanionId { get; private set; }

    public int CompanionUnreadMessages { get; private set; }

    public IReadOnlyCollection<PersonalChatMessage> Messages => _messages.AsReadOnly();

    public void UpdateInitiatorUnreadMessageCount(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1, nameof(count));

        if (InitiatorUnreadMessages != count)
        {
            InitiatorUnreadMessages = count;
        }
    }
    public void UpdateCompanionUnreadMessageCount(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 1, nameof(count));

        if (CompanionUnreadMessages != count)
        {
            CompanionUnreadMessages = count;
        }
    }

}
