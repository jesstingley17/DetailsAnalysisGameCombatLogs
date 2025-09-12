using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public class PersonalChatModel
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [StringLength(8)]
    public string InitiatorId { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int InitiatorUnreadMessages { get; set; }

    [Required]
    [StringLength(8)]
    public string CompanionId { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CompanionUnreadMessages { get; set; }
}
