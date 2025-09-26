namespace CombatAnalysis.NotificationAPI.Kafka.Actions;

public class NotificationAction
{
    public int NotificationId { get; set; }

    public string RecipientId { get; set; } = string.Empty;

    public int State { get; set; }

    public string When { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;
}
