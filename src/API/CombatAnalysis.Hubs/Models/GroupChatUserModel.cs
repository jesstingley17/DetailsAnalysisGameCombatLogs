namespace CombatAnalysis.Hubs.Models;

public record GroupChatUserModel(
    string Id,
    string Username,
    int GroupChatId,
    int UnreadMessages,
    string? AppUserId
    );
