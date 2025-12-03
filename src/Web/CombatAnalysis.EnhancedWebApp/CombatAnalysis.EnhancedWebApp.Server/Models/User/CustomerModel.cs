namespace CombatAnalysis.EnhancedWebApp.Server.Models.User;

public class CustomerModel
{
    public string Id { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public int PostalCode { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
