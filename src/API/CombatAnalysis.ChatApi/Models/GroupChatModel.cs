using Chat.Domain.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public record GroupChatModel(
    [Range(0, int.MaxValue)] int Id, 
    [Required][StringLength(GroupChat.MaxNameLength)] string Name,
    [Required] string OwnerId
    );