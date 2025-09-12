using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Requests;

public class GroupChatMessageRequest
{
    [Range(1, int.MaxValue)]
    public int ChatId { get; set; }

    [Required]
    public string GroupChatUserId { get; set; } = string.Empty;

    [Range(1, 100)]
    public int PageSize { get; set; }
}
