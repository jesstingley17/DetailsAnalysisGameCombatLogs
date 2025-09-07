namespace CombatAnalysis.IdentityDAL.Entities;

public class RefreshToken
{
    public string Id { get; set; }

    public string TokenHash { get; set; }

    public string TokenSalt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset ExpiresAt { get; set; }

    public DateTimeOffset? RevokedAt { get; set; }

    public string ClientId { get; set; }

    public string UserId { get; set; }

    public string? ReplacedByTokenId { get; set; }
}
