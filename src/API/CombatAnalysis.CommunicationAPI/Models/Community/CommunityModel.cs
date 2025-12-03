using CombatAnalysis.CommunicationBL.Enums;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Community;

public record CommunityModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Name,
    [Required] string Description,
    [Range((int)CommunityPolicyType.Public, (int)CommunityPolicyType.Private)] CommunityPolicyType PolicyType,
    [Required] string AppUserId
    );
