namespace CombatAnalysis.UserApi.Models;

public class FriendModel
{
    public int Id { get; set; }

    public string WhoFriendId { get; set; } = string.Empty;

    public string WhoFriendUsername { get; set; } = string.Empty;

    public string ForWhomId { get; set; } = string.Empty;

    public string ForWhomUsername { get; set; } = string.Empty;
}
