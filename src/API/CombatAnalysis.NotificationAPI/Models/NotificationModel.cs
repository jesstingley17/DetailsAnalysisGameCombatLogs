namespace CombatAnalysis.NotificationAPI.Models;

public class NotificationModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ReadAt { get; set; }

    public int Type { get; set; }

    public int Status { get; set; }

    public string InitiatorId { get; set; } = string.Empty;

    public string? TargetName { get; set; }
}
