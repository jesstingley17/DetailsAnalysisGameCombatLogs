using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Models;

public record GroupChatUserModel(
        [Required] string Id,
        [Required][StringLength(8)] string Username,
        [Range(0, int.MaxValue)] int UnreadMessages,
        [Range(1, int.MaxValue)] int? LastReadMessageId,
        [Range(1, int.MaxValue)] int GroupChatId,
        [Required] string AppUserId
    );
