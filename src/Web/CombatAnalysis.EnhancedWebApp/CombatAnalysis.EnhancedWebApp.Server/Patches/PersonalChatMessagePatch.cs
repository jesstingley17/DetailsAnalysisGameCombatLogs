namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record PersonalChatMessagePatch(
        int Id,
        string? Message,
        int? Status,
        int? MarkedType
    );