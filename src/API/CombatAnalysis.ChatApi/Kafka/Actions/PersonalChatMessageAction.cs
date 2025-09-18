using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Models;

namespace CombatAnalysis.ChatApi.Kafka.Actions;

public class PersonalChatMessageAction
{
    public PersonalChatMessageModel ChatMessage { get; set; } = new();

    public ChatMessageActionState State { get; set; }

    public DateTimeOffset When { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;
}
