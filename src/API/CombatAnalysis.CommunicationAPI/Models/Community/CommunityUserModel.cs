using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Community;

public record CommunityUserModel(
    [Required] string Id,
    [Required] string Username,
    [Required] string AppUserId,
    [Range(0, int.MaxValue)] int CommunityId
    );
