namespace CombatAnalysis.ChatApi.Interfaces;

public interface IChatHubHelper
{
    Task ConnectToUnreadMessageHubAsync(string hubURL, string refreshToken, string accessToken);

    Task JoinChatRoomAsync(int chatId);

    Task RequestUnreadMessagesAsync(int chatId, string appUserId);
}
