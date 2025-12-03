using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Models;

public record VoiceChatModel(
    [Required] string Id,
    [Required] string AppUserId
    );