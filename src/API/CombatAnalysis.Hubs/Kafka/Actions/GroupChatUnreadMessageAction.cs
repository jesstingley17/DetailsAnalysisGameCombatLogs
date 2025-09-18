using CombatAnalysis.Hubs.Enums;

namespace CombatAnalysis.Hubs.Kafka.Actions;

public class GroupChatUnreadMessageAction
{
    public int ChatId { get; set; }

    public int MessageId { get; set; }

    public string GroupChatUserId { get; set; }

    public ChatMessageActionState State { get; set; }

    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }
}
