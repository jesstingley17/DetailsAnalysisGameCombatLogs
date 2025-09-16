using Chat.Domain.Enums.GroupChatRules;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public record GroupChatRulesModel(
    [Range(0, int.MaxValue)] int Id,
    [Range((int)InvitePeopleRestrictions.Anyone, (int)InvitePeopleRestrictions.Owner)] InvitePeopleRestrictions InvitePeople,
    [Range((int)RemovePeopleRestrictions.Anyone, (int)RemovePeopleRestrictions.Owner)] RemovePeopleRestrictions RemovePeople,
    [Range((int)PinMessageRestrictions.Anyone, (int)PinMessageRestrictions.Owner)] PinMessageRestrictions PinMessage,
    [Range((int)AnnouncementsRestrictions.Anyone, (int)AnnouncementsRestrictions.Owner)] AnnouncementsRestrictions Announcements,
    [Range(1, int.MaxValue)] int GroupChatId
    );
