using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class VoiceChat: IRepositoryEntity<VoiceChatId>
{
    private VoiceChat() { }

    public VoiceChat(VoiceChatId id, UserId appUserId)
    {
        Id = id;
        AppUserId = appUserId;
    }

    public VoiceChatId Id { get; private set; }

    public UserId AppUserId { get; private set; }
}