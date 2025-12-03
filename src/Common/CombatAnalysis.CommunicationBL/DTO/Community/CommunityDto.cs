using CombatAnalysis.CommunicationBL.Enums;

namespace CombatAnalysis.CommunicationBL.DTO.Community;

public record CommunityDto(
    int Id,
    string Name,
    string Description,
    CommunityPolicyType PolicyType,
    string AppUserId
    );