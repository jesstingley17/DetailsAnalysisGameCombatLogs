using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public class VoiceChatModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

    [Required]
    public string AppUserId { get; set; } = string.Empty;
}
