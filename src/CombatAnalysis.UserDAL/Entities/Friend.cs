namespace CombatAnalysis.UserDAL.Entities;

public record Friend(
    int Id,
    string WhoFriendId,
    string ForWhomId
    );
