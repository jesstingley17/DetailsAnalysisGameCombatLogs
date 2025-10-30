namespace CombatAnalysis.CommunicationBL.DTO.Community;

public record CommunityDiscussionDto(
    int Id,
    string Title,
    string Content,
    DateTimeOffset When,
    string AppUserId,
    int CommunityId
    );
