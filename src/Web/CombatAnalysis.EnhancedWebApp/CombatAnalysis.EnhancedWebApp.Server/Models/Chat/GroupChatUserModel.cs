namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record GroupChatUserModel(
    string Id,
    string Username,
    int UnreadMessages,
    int? LastReadMessageId,
    int GroupChatId,
    string AppUserId
    );
