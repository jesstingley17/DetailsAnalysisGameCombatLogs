namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record PersonalChatMessageModel(
    int Id,
    string Username,
    string Message,
    DateTimeOffset Time,
    int Status,
    int Type,
    int MarkedType,
    bool IsEdited,
    int PersonalChatId,
    string AppUserId
    );