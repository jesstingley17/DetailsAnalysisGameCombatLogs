using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.UserApi.Models;

public record CustomerModel(
    [Required] string Id,
    [Required] string Country,
    [Required] string City,
    [Required] int PostalCode,
    [Required] string AppUserId
    );
