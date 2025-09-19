namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record PersonalChatPatch(
        int Id,
        int? InitiatorUnreadMessages,
        int? CompanionUnreadMessages
    );
