namespace Chat.Domain.ValueObjects;

public record GroupChatId
{
    public GroupChatId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(GroupChatId id) => id.Value;

    public static implicit operator GroupChatId(int value) => new(value);
}
