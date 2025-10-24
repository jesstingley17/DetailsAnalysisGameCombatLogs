namespace CombatAnalysis.UserDAL.Entities;

public record BannedUser(
    int Id,
    string BannedCustomerId,
    string AppUserId
    );
