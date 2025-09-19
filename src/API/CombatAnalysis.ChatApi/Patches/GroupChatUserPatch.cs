using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Patches;

public record GroupChatUserPatch(
        [Required] string Id,
        [Range(1, int.MaxValue)] int? LastReadMessageId,
        [Range(0, int.MaxValue)] int? UnreadMessages
    );
