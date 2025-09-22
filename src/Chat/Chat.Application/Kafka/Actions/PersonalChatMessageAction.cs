using Chat.Application.DTOs;
using Chat.Application.Enums;
using Chat.Application.Kafka.Security;

namespace Chat.Application.Kafka.Actions;

public class PersonalChatMessageAction : SecurityAction
{
    public PersonalChatMessageDto ChatMessage { get; set; }

    public ChatMessageActionState State { get; set; }

    public DateTimeOffset When { get; set; }
}
