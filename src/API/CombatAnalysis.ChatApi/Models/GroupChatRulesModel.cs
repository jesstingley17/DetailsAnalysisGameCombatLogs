using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public class GroupChatRulesModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, 100)]
    public int InvitePeople { get; set; }

    [Range(0, 100)]
    public int RemovePeople { get; set; }

    [Range(0, 100)]
    public int PinMessage { get; set; }

    [Range(0, 100)]
    public int Announcements { get; set; }

    [Range(1, int.MaxValue)]
    public int ChatId { get; set; }
}
