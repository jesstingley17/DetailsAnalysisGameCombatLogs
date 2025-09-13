using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class VoiceChat
{
    private VoiceChat() { }

    public VoiceChat(string id, UserId userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("UserId cannot be empty");
        }

        Id = id;
        AppUserId = userId;
    }

    public VoiceChatId Id { get; set; }

    public UserId AppUserId { get; set; }
}