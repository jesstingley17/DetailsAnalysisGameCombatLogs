using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.UserAPI.Models;

public record CustomerModel(
    [Required] string Id,
    [Required] string Country,
    [Required] string City,
    [Required] int PostalCode,
    [Required] string AppUserId
    );
