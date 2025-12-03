namespace CombatAnalysis.NotificationAPI.Consts;

public static class KafkaTopics
{
    public static string PersonalChatMessage { get; } = "personal-chat-message";

    public static string GroupChatMessage { get; } = "group-chat-message";

    public static string PersonalChat { get; } = "personal-chat";

    public static string GroupChat { get; } = "group-chat";

    public static string Notification { get; } = "notification";
}
