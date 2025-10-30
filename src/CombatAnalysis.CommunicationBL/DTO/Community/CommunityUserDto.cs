namespace CombatAnalysis.CommunicationBL.DTO.Community;

public record CommunityUserDto(
    string Id,
    string Username,
    string AppUserId,
    int CommunityId
    );
