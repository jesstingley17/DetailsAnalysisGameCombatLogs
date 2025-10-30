namespace CombatAnalysis.UserBL.DTO;

public record FriendCreateDto(
    int Id,
    string WhoFriendId,
    string ForWhomId
    );
