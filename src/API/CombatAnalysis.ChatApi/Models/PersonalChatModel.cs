using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Models;

public class PersonalChatModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string InitiatorId { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int InitiatorUnreadMessages { get; set; }

    [Required]
    public string CompanionId { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int CompanionUnreadMessages { get; set; }
}
