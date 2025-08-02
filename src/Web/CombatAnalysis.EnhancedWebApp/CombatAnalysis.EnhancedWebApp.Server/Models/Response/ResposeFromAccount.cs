using CombatAnalysis.EnhancedWebApp.Server.Models.User;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.Response;

public struct ResponseFromAccount
{
    public AppUserModel User { get; set; }

    public string RefreshToken { get; set; }
}
