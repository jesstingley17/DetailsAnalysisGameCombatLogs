using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.UserAPI.Models;

public record FriendModel(
    [Required] int Id,
    [Required] string WhoFriendId,
    [Required] string ForWhomId
    );
