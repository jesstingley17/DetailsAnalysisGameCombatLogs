using Chat.Domain.Enums;

namespace Chat.Domain.Exceptions;

public class VoiceChatNotFoundException(string voiceChatId) : DomainException($"Voice chat with Id '{voiceChatId}' was not found.", ExceptionCode.NotFound)
{
    public string VoiceChatId { get; } = voiceChatId;
}
