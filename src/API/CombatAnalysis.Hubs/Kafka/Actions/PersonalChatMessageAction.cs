namespace CombatAnalysis.Hubs.Kafka.Actions;

public class PersonalChatMessageAction
{
    public int ChatId { get; set; }

    public string InititatorId { get; set; } = string.Empty;

    public string InititatorUsername { get; set; } = string.Empty;

    public string RecipientId { get; set; } = string.Empty;

    public int State { get; set; }

    public string When { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;
}
