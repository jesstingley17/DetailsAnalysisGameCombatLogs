namespace CombatAnalysis.ChatApi.Models.Kafka;

public class PersonalChatMessageAction
{
    public int ChatId { get; set; }

    public string AppUserId { get; set; }

    public string CompanionId { get; set; }

    public int State { get; set; }

    public string When { get; set; }
}
