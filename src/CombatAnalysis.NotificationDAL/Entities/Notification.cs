namespace CombatAnalysis.NotificationDAL.Entities;

public class Notification
{
    public int Id { get; set; }

    public int Type { get; set; }

    public int Status { get; set; }

    public string InitiatorId { get; set; } = string.Empty;

    public string? InitiatorName { get; set; }

    public string RecipientId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
