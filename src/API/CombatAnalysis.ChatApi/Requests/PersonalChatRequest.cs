using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Requests;

public class PersonalChatRequest
{
    [Range(1, int.MaxValue)]
    public int ChatId { get; set; }

    [Range(1, 100)]
    public int Page { get; set; }

    [Range(1, 100)]
    public int PageSize { get; set; }
}
