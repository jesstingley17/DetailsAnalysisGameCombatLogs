namespace CombatAnalysis.ChatApi.Models.Kafka;

public class MessageCreatedModel
{
    public int ChatId { get; set; }

    public string AppUserId { get; set; }

    public string CompanionId { get; set; }
}
