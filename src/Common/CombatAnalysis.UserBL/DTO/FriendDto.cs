namespace CombatAnalysis.UserBL.DTO;

public record FriendDto(
    int Id,
    string WhoFriendId,
    string WhoFriendUsername,
    string ForWhomId,
    string ForWhomUsername
    );
