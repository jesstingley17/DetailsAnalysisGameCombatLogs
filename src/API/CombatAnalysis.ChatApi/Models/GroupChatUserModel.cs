using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public record GroupChatUserModel(
    [Required] string Id,
    [Required][StringLength(8)] string Username,
    [Range(0, int.MaxValue)] int UnreadMessages,
    [Range(1, int.MaxValue)] int GroupChatId,
    [Required][StringLength(8)] string AppUserId
    );
