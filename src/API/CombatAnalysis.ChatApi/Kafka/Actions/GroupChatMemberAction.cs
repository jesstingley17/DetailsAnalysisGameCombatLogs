using CombatAnalysis.ChatApi.Models;

namespace CombatAnalysis.ChatApi.Kafka.Actions;

public class GroupChatMemberAction
{
    public GroupChatUserModel User { get; set; }

    public int State { get; set; }

    public string When { get; set; }

    public string RefreshToken { get; set; }

    public string AccessToken { get; set; }
}
