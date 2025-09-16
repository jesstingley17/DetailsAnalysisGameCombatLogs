using Chat.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.ChatApi.Models;

public record GroupChatMessageModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] [StringLength(32)] string Username,
    [Required] [StringLength(256)] string Message,
    [Required] DateTimeOffset Time,
    [Range((int)MessageStatus.Sent, (int)MessageStatus.Read)] MessageStatus Status,
    [Range((int)MessageType.Default, (int)MessageType.System)] MessageType Type,
    [Range((int)MessageMarkedType.None, (int)MessageMarkedType.Emotions)] MessageMarkedType MarkedType,
    [Required] bool IsEdited,
    [Range(1, int.MaxValue)] int GroupChatId,
    [Required] string GroupChatUserId
    );
