namespace Chat.Domain.ValueObjects;

public class GroupChatMessageId(int value)
{
    public int Value { get; } = value;

    public bool HasValue => Value > 0;

    public static implicit operator int(GroupChatMessageId id) => id.Value;

    public static implicit operator GroupChatMessageId(int value) => new(value);
}
