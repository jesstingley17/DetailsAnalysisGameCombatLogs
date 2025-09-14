using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class VoiceChat: IRepositoryEntity<VoiceChatId>
{
    private VoiceChat() { }

    public VoiceChat(string id, UserId userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));

        Id = id;
        AppUserId = userId;
    }

    public VoiceChatId Id { get; set; }

    public UserId AppUserId { get; set; }
}