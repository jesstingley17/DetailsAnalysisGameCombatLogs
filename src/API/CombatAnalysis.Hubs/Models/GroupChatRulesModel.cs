namespace CombatAnalysis.Hubs.Models;

public record GroupChatRulesModel(
    int Id,
    int InvitePeople,
    int RemovePeople,
    int PinMessage,
    int Announcements,
    int ChatId
    );
