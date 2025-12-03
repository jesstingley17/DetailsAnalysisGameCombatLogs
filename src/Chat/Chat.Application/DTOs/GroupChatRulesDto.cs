using Chat.Domain.Enums.GroupChatRules;

namespace Chat.Application.DTOs;

public class GroupChatRulesDto
{
    public int Id { get; set; }

    public InvitePeopleRestrictions InvitePeople { get; set; }

    public RemovePeopleRestrictions RemovePeople { get; set; }

    public PinMessageRestrictions PinMessage { get; set; }

    public AnnouncementsRestrictions Announcements { get; set; }

    public int GroupChatId { get; set; }
}
