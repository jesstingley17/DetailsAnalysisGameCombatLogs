using Chat.Domain.ValueObjects;

namespace Chat.Domain.Interfaces;

internal interface IChatEntity
{
    public GroupChatId GroupChatId { get; }
}
