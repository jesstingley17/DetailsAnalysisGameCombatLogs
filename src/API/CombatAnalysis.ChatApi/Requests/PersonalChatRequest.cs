using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Requests;

public record PersonalChatRequest(
    [Range(1, int.MaxValue)] int ChatId, 
    [Range(1, 100)] int Page,
    [Range(1, 100)] int PageSize
    );
