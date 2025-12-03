namespace Chat.Domain.ValueObjects;

public record PersonalChatMessageId
{
    public PersonalChatMessageId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(PersonalChatMessageId id) => id.Value;

    public static implicit operator PersonalChatMessageId(int value) => new(value);
}
