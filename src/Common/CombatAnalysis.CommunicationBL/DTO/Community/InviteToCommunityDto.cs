namespace CombatAnalysis.CommunicationBL.DTO.Community;

public record InviteToCommunityDto(
    int Id,
    int CommunityId,
    string ToAppUserId,
    DateTimeOffset When,
    string AppUserId
    );
