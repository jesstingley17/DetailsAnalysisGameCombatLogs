namespace CombatAnalysis.Hubs.Kafka.Actions;

public class NotificationAction
{
    public int NotificationId { get; set; }

    public string RecipientId { get; set; } = string.Empty;

    public int State { get; set; }

    public DateTimeOffset When { get; set; }

    public string AccessToken { get; set; } = string.Empty;
}
