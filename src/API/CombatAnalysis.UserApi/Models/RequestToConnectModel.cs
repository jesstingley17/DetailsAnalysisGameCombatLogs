using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.UserAPI.Models;

public record RequestToConnectModel(
    [Required] int Id,
    [Required] string ToAppUserId,
    [Required] DateTimeOffset When,
    [Required] string AppUserId
    );
