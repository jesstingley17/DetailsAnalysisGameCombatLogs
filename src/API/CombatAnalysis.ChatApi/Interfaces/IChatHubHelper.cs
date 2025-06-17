namespace CombatAnalysis.ChatApi.Interfaces;

public interface IChatHubHelper
{
    Task ConnectToUnreadMessageHubAsync(string hubURL, string refreshToken, string accessToken);

    Task JoinRoomAsync(int chatId);

    Task RequestUnreadMessagesAsync(int chatId, string appUserId);

    Task RequestsChats(int chatId, string appUserId);
}
