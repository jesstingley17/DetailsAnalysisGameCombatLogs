using Chat.Application.DTOs;

namespace CombatAnalysis.Hubs.Models.Containers;

public class GroupChatContainerModel
{
    public GroupChatDto GroupChat { get; set; }

    public GroupChatRulesDto GroupChatRules { get; set; }

    public GroupChatUserDto GroupChatUser { get; set; }
}
