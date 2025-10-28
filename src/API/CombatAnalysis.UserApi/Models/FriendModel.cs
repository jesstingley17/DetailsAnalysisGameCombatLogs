using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.UserApi.Models;

public record FriendModel(
    [Required] int Id,
    [Required] string WhoFriendId,
    [Required] string ForWhomId
    );
