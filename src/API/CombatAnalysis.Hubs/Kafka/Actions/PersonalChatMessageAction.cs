namespace CombatAnalysis.Hubs.Kafka.Actions;

public class PersonalChatMessageAction
{
    public int ChatId { get; set; }

    public string AppUserId { get; set; }

    public int State { get; set; }

    public string When { get; set; }

    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }
}
