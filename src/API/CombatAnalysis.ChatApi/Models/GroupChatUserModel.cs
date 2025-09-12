using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public class GroupChatUserModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(8)]
    public string Username { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int UnreadMessages { get; set; }

    [Range(1, int.MaxValue)]
    public int ChatId { get; set; }

    [Required]
    [StringLength(8)]
    public string AppUserId { get; set; } = string.Empty;
}
