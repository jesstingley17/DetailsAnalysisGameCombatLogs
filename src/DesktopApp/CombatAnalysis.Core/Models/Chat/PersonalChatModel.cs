namespace CombatAnalysis.Core.Models.Chat;

public class PersonalChatModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string InitiatorId { get; set; }

    public int InitiatorUnreadMessages { get; set; }

    public string CompanionId { get; set; }

    public int CompanionUnreadMessages { get; set; }

    public int CurrentUnreadMessages { get; set; }
}
