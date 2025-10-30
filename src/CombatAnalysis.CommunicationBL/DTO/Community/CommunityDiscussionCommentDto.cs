namespace CombatAnalysis.CommunicationBL.DTO.Community;

public record CommunityDiscussionCommentDto(
    int Id,
    string Content,
    DateTimeOffset When,
    string AppUserId,
    int CommunityDiscussionId
    );
