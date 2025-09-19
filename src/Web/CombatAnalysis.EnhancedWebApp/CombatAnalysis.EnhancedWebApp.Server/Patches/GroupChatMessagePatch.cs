namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record GroupChatMessagePatch(
        int Id,
        string? Message,
        int? Status,
        int? MarkedType
    );