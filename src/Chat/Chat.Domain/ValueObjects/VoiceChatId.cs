namespace Chat.Domain.ValueObjects;

public record VoiceChatId
{
    public VoiceChatId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("VoiceChatId cannot be empty");
        }

        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(VoiceChatId id) => id.Value;


    public static implicit operator VoiceChatId(string value) => new(value);
}
