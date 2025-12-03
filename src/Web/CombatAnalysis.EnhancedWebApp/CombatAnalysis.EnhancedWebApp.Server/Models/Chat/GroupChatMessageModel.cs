namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record GroupChatMessageModel(
    int Id,
    string Username,
    string Message,
    DateTimeOffset Time,
    int Status,
    int Type,
    int MarkedType,
    bool IsEdited,
    int GroupChatId,
    string GroupChatUserId
    );