namespace CombatAnalysis.UserBL.DTO;

public record CustomerDto(
    string Id,
    string Country,
    string City,
    int PostalCode,
    string AppUserId
    );
