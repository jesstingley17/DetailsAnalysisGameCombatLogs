using Chat.Domain.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public record GroupChatModel(
    [Range(0, int.MaxValue)] int Id, 
    [Required] [StringLength(GroupChat.NAME_MAX_LENGTH)] string Name,
    [Required] string OwnerId
    );