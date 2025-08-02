using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.Containers;

public class GroupChatContainerModel
{
    public GroupChatModel GroupChat { get; set; }

    public GroupChatRulesModel GroupChatRules { get; set; }

    public GroupChatUserModel GroupChatUser { get; set; }
}
