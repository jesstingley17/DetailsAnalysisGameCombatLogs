using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class VoiceChat: IRepositoryEntity<VoiceChatId>
{
    private VoiceChat() { }

    public VoiceChat(UserId userId)
    {
        AppUserId = userId;
    }

    public VoiceChatId Id { get; private set; }

    public UserId AppUserId { get; private set; }
}