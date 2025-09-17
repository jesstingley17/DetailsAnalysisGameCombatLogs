using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Patches;

public record GroupChatPatch(
    [Range(0, int.MaxValue)] int Id,
    string? Name,
    string? OwnerId
);
