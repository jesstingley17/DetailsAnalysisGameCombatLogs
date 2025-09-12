using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public class UnreadGroupChatMessageModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Required]
    public string GroupChatUserId { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int GroupChatMessageId { get; set; }
}
