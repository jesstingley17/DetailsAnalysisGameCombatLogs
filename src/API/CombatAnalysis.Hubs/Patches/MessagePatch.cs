namespace CombatAnalysis.Hubs.Patches;

public record MessagePatch(
    int Id,
    string Message,
    int Status,
    int MarkedType,
    int ChatId
    );
