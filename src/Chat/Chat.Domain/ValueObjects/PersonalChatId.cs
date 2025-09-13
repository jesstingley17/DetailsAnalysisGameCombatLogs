namespace Chat.Domain.ValueObjects;

public record PersonalChatId
{
    public PersonalChatId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(PersonalChatId id) => id.Value;

    public static implicit operator PersonalChatId(int value) => new(value);
}
