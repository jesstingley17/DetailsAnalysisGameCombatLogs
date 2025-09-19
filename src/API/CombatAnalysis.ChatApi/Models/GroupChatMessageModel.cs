using Chat.Domain.Entities;
using Chat.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public record GroupChatMessageModel(
    int Id,
    [Required] [StringLength(GroupChatMessage.MESSAGE_MAX_LENGTH)] string Username,
    [Required] [StringLength(GroupChatMessage.MESSAGE_MAX_LENGTH)] string Message,
    [Required] DateTimeOffset Time,
    [Range((int)MessageStatus.Sending, (int)MessageStatus.Read)] MessageStatus Status,
    [Range((int)MessageType.Default, (int)MessageType.Log)] MessageType Type,
    [Range((int)MessageMarkedType.None, (int)MessageMarkedType.Emotions)] MessageMarkedType MarkedType,
    [Required] bool IsEdited,
    [Range(1, int.MaxValue)] int GroupChatId,
    [Required] string GroupChatUserId
    );
