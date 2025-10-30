namespace CombatAnalysis.UserDAL.Entities;

public record RequestToConnect(
    int Id,
    string ToAppUserId,
    DateTimeOffset When,
    string AppUserId
    );
