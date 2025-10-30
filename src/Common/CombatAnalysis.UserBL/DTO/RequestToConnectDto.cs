namespace CombatAnalysis.UserBL.DTO;

public record RequestToConnectDto(
    int Id,
    string ToAppUserId,
    DateTimeOffset When,
    string AppUserId
    );
