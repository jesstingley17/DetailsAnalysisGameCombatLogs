using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Models;

namespace CombatAnalysis.Hubs.Kafka.Actions;

public class PersonalChatMessageAction
{
    public PersonalChatMessageModel ChatMessage { get; set; }

    public ChatMessageActionState State { get; set; }

    public DateTimeOffset When { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;
}