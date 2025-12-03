using Microsoft.AspNetCore.Identity;

namespace CombatAnalysisIdentity.Models;

public class IdentityUserModel : IdentityUser
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Salt { get; set; }
}
