using CombatAnalysis.ChatApi.Enums;

namespace CombatAnalysis.ChatApi.Kafka.Actions;

public class PersonalChatMessageAction
{
    public int ChatId { get; set; }

    public string InititatorId { get; set; } = string.Empty;

    public string InititatorUsername { get; set; } = string.Empty;

    public string RecipientId { get; set; } = string.Empty;

    public ChatMessageActionState State { get; set; }

    public DateTimeOffset When { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;
}
