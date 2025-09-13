namespace Chat.Domain.ValueObjects;

public record GroupChatRulesId
{
    public GroupChatRulesId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(GroupChatRulesId id) => id.Value;

    public static implicit operator GroupChatRulesId(int value) => new(value);
}
