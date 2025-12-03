namespace CombatAnalysis.Hubs.Models;

public class PersonalChatModel
{
    public int Id { get; set; }

    public string InitiatorId { get; set; }

    public int InitiatorUnreadMessages { get; set; }

    public string CompanionId { get; set; }

    public int CompanionUnreadMessages { get; set; }
}
