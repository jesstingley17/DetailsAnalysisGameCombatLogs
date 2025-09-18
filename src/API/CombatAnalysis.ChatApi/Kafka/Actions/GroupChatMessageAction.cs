using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Models;

namespace CombatAnalysis.ChatApi.Kafka.Actions;

public class GroupChatMessageAction
{
    public string InitiatorGroupChatUserId { get; set; }

    public GroupChatMessageModel ChatMessage { get; set; }

    public ChatMessageActionState State { get; set; }

    public DateTimeOffset When { get; set; }

    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }
}
