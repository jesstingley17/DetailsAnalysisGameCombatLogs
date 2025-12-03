namespace Chat.Domain.ValueObjects;

public record GroupChatUserId
{
    public GroupChatUserId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("GroupChatUserId cannot be empty");
        }

        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(GroupChatUserId id) => id.Value;

    public static implicit operator GroupChatUserId(string value) => new(value);
}
