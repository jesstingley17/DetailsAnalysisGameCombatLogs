namespace CombatAnalysis.NotificationAPI.Interfaces;

public interface IChatHubHelper
{
    Task ConnectToHubAsync(string hubURL, string refreshToken, string accessToken);

    Task JoinRoomAsync(string appUserId);

    Task RequestNotificationAsync(int notificationId, int chatId);

    Task RequestRecipientNotifications(string recipientId);
}
