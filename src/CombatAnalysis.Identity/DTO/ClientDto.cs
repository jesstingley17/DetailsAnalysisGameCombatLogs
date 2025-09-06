namespace CombatAnalysis.Identity.DTO;

public class ClientDto
{
    public string Id { get; set; } = string.Empty;

    public string RedirectUrl { get; set; } = string.Empty;

    public string AllowedScopes { get; set; } = string.Empty;

    public string AllowedAudiences { get; set; } = string.Empty;

    public string ClientName { get; set; } = string.Empty;

    public int ClientType { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? ClientSecret { get; set; }
}
