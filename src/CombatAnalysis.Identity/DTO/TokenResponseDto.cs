namespace CombatAnalysis.Identity.DTO;

public class TokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;

    public string TokenType { get; set; } = string.Empty;

    public DateTimeOffset Expires { get; set; }

    public RefreshTokenResponseDto RefreshToken { get; set; }
}
