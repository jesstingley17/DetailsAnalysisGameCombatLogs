namespace Chat.Domain.Exceptions;

public class VoiceChatNotFoundException(string voiceChatId) : DomainException($"Voice chat with Id '{voiceChatId}' was not found.")
{
    public string VoiceChatId { get; } = voiceChatId;
}
