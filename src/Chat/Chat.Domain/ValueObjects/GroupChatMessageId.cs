namespace Chat.Domain.ValueObjects;

public class GroupChatMessageId
{
    public GroupChatMessageId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public bool HasValue => Value > 0;

    public static implicit operator int(GroupChatMessageId id) => id.Value;

    public static implicit operator GroupChatMessageId(int value) => new(value);
}
