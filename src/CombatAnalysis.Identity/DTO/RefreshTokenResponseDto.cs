namespace CombatAnalysis.Identity.DTO;

public class RefreshTokenResponseDto
{
    public string Id { get; set; }

    public string Token { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }
}
