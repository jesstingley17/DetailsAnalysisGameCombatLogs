namespace CombatAnalysis.UserBL.DTO;

public record BannedUserDto(
    int Id,
    string WhomBannedId,
    string BannedUserId
    );