namespace CombatAnalysis.UserBL.DTO;

public record BannedUserDto(
    int Id,
    string BannedCustomerId,
    string CustomerId
    );