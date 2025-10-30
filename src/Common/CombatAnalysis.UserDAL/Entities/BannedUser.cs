namespace CombatAnalysis.UserDAL.Entities;

public record BannedUser(
    int Id,
    string WhomBannedId,
    string BannedUserId
    );
