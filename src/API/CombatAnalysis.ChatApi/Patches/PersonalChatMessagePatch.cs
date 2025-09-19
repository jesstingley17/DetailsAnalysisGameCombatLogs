using Chat.Domain.Entities;
using Chat.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Patches;

public record PersonalChatMessagePatch(
        [Required] int Id,
        [StringLength(PersonalChatMessage.MESSAGE_MAX_LENGTH, MinimumLength = 1)] string? Message,
        MessageStatus? Status,
        MessageMarkedType? MarkedType
    );