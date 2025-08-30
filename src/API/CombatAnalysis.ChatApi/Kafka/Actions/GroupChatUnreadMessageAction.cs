namespace CombatAnalysis.ChatApi.Kafka.Actions;

public class GroupChatUnreadMessageAction
{
    public int ChatId { get; set; }

    public int MessageId { get; set; }

    public string GroupChatUserId { get; set; }

    public int State { get; set; }

    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }
}
