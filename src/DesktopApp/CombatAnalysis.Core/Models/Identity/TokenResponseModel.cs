namespace CombatAnalysis.Core.Models.Identity;

public class TokenResponseModel
{
    public string AccessToken { get; set; } = string.Empty;

    public string TokenType { get; set; } = string.Empty;

    public DateTimeOffset Expires { get; set; }

    public RefreshTokenResponseModel RefreshToken { get; set; }
}
