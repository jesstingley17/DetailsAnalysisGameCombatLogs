using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.UserApi.Models;

public record AppUserModel(
    [Required] string Id,
    [Required] string Username,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] int PhoneNumber,
    [Required] DateTimeOffset Birthday,
    string? AboutMe,
    [Required] int Gender,
    [Required] string IdentityUserId
    );
