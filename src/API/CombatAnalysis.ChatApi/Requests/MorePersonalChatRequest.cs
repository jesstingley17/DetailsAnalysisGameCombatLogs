using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Requests;

public class MorePersonalChatRequest
{
    [Range(1, int.MaxValue)]
    public int ChatId { get; set; }

    [Range(1, 100)]
    public int Offset { get; set; }

    [Range(1, 100)]
    public int PageSize { get; set; }
}
