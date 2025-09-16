using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public class PersonalChatMessageModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    [StringLength(32)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    public string Message { get; set; } = string.Empty;

    [Required]
    public DateTimeOffset Time { get; set; }

    [Range(0, 100)]
    public int Status { get; set; }

    [Range(0, 100)]
    public int Type { get; set; }

    [Range(0, 100)]
    public int MarkedType { get; set; }

    [Required]
    public bool IsEdited { get; set; }

    [Range(1, int.MaxValue)]
    public int PersonalChatId { get; set; }

    [Required]
    public string AppUserId { get; set; } = string.Empty;
}
