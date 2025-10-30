using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.UserAPI.Models;

public record BannedUserModel(
    [Required] int Id,
    [Required] string WhomBannedId,
    [Required] string BannedUserId
    );
