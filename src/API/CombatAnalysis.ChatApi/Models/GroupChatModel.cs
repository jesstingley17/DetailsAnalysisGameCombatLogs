using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public class GroupChatModel
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [StringLength(8)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(8)]
    public string AppUserId { get; set; } = string.Empty;
}
