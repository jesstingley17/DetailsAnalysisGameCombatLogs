namespace CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

public class PersonalChatModel
{
    public int Id { get; set; }

    public string InitiatorId { get; set; } = string.Empty;

    public int InitiatorUnreadMessages { get; set; }

    public string CompanionId { get; set; } = string.Empty;

    public int CompanionUnreadMessages { get; set; }
}
