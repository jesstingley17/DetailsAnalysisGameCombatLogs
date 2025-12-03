using Chat.Domain.Entities;
using Chat.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatAPI.Patches;

public record GroupChatMessagePatch(
        [Required] int Id,
        [StringLength(GroupChatMessage.MESSAGE_MAX_LENGTH, MinimumLength = 1)] string? Message,
        MessageStatus? Status,
        MessageMarkedType? MarkedType
    );