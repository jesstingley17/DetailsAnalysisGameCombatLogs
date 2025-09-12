using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public class GroupChatModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [StringLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string AppUserId { get; set; } = string.Empty;
}
