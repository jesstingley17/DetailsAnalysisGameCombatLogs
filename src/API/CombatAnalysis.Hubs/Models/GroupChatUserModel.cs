namespace CombatAnalysis.Hubs.Models;

public record GroupChatUserModel(
    string Id,
    string Username,
    int UnreadMessages,
    int? LastReadMessageId,
    int GroupChatId,
    string AppUserId
    );
