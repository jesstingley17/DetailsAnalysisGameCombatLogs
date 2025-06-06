namespace CombatAnalysis.ChatApi.Models.Kafka;

public class GroupChatMessageAction
{
    public int ChatId { get; set; }

    public string GroupChatUserId { get; set; }

    public int State { get; set; }

    public string When { get; set; }
}
