using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.NotificationAPI.Models;

public record NotificationModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] int Type,
    [Required] int Status,
    [Required] string InitiatorId,
    string? InitiatorName,
    [Required] string RecipientId,
    [Required] DateTime CreatedAt,
    DateTime? ReadAt
    );
