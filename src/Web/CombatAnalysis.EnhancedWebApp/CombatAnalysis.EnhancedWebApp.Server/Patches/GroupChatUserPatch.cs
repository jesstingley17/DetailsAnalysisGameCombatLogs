namespace CombatAnalysis.EnhancedWebApp.Server.Patches;

public record GroupChatUserPatch(
        string Id,
        int? LastReadMessageId,
        int? UnreadMessages
    );
