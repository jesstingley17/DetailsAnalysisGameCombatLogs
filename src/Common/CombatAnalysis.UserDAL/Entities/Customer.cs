namespace CombatAnalysis.UserDAL.Entities;

public record Customer(
    string Id,
    string Country,
    string City,
    int PostalCode,
    string AppUserId
    );