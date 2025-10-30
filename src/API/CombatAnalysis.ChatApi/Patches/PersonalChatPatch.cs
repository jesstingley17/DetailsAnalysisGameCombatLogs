using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Patches;

public record PersonalChatPatch(
    [Required] int Id,
    [Range(0, int.MaxValue)] int? InitiatorUnreadMessages,
    [Range(0, int.MaxValue)] int? CompanionUnreadMessages
    );
