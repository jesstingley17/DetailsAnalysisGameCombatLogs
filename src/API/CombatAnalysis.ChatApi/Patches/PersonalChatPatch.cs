using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Patches;

public record PersonalChatPatch(
    [Required] int Id,
    int? InitiatorUnreadMessages,
    int? CompanionUnreadMessages
    );
