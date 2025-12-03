namespace Chat.Domain.ValueObjects;

public record UserId
{
    public UserId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("UserId cannot be empty");
        }

        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(UserId id) => id.Value;


    public static implicit operator UserId(string value) => new(value);
}
