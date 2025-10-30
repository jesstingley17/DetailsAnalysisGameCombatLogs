using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public record VoiceChatModel(
    [Required] string Id,
    [Required] string AppUserId
    );