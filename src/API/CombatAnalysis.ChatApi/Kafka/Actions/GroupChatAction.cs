using CombatAnalysis.ChatApi.Models;

namespace CombatAnalysis.ChatApi.Kafka.Actions;

public class GroupChatAction
{
    public int ChatId { get; set; }

    public GroupChatRulesModel Rules { get; set; }

    public GroupChatUserModel User { get; set; }

    public int State { get; set; }

    public string When { get; set; }
}
