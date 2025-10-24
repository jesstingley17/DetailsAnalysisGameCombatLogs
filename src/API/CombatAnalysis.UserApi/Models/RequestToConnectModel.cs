using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.UserApi.Models;

public record RequestToConnectModel(
    [Required] int Id,
    [Required] string ToAppUserId,
    [Required] DateTimeOffset When,
    [Required] string AppUserId
    );
