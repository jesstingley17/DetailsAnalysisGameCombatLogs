using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public class PersonalChatMessageModel
{
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [StringLength(8)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(1)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [StringLength(8)]
    public string Time { get; set; } = string.Empty;

    [Range(1, 100)]
    public int Status { get; set; }

    [Range(1, 100)]
    public int Type { get; set; }

    [Range(1, 100)]
    public int MarkedType { get; set; }

    [Required]
    public bool IsEdited { get; set; }

    [Range(1, int.MaxValue)]
    public int ChatId { get; set; }

    [Range(1, int.MaxValue)]
    [StringLength(8)]
    public string AppUserId { get; set; } = string.Empty;
}
