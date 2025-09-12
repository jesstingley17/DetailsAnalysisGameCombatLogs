using CombatAnalysis.ChatApi.Models;

namespace CombatAnalysis.ChatApi.Kafka.Actions;

public class GroupChatMessageAction
{
    public GroupChatMessageModel Message { get; set; }

    public int State { get; set; }

    public DateTimeOffset When { get; set; }

    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }
}
