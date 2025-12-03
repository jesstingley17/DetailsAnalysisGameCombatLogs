namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public record PersonalChatModel(
    int Id,
    string InitiatorId,
    int InitiatorUnreadMessages,
    string CompanionId,
    int CompanionUnreadMessages
    );
