namespace CombatAnalysis.Core.Models.Identity;

public class RefreshTokenResponseModel
{
    public string Id { get; set; }

    public string Token { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }
}
