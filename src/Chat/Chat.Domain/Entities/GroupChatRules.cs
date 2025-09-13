using Chat.Domain.Enums.GroupChatRules;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class GroupChatRules
{
    private GroupChatRules() { }

    public GroupChatRules(int chatId, 
        InvitePeopleRestrictions invitePeople = InvitePeopleRestrictions.Anyone, 
        RemovePeopleRestrictions removePeople = RemovePeopleRestrictions.Anyone,
        PinMessageRestrictions pinMessage = PinMessageRestrictions.Anyone,
        AnnouncementsRestrictions announcements = AnnouncementsRestrictions.Anyone)
    {
        InvitePeople = invitePeople;
        RemovePeople = removePeople;
        PinMessage = pinMessage;
        Announcements = announcements;
        GroupChatId = chatId;
    }

    public GroupChatRulesId Id { get; private set; }

    public InvitePeopleRestrictions InvitePeople { get; private set; }

    public RemovePeopleRestrictions RemovePeople { get; private set; }

    public PinMessageRestrictions PinMessage { get; private set; }

    public AnnouncementsRestrictions Announcements { get; private set; }

    public GroupChatId GroupChatId { get; private set; }
}
