namespace CombatAnalysis.ChatApi.Interfaces;

public interface IChatHubHelper
{
    Task ConnectToHubAsync(string hubUrl, string accessToken);

    Task JoinRoomAsync(int chatId);

    Task RequestUnreadMessagesAsync(int chatId, string appUserId);

    Task RequestUnreadMessagesAsync(int chatId);

    Task SendMessageReadAsync(int chatId, int chatMessageId);

    Task RequestsChatsAsync(int chatId, string appUserId);

    Task RequestMessageAsync<T>(int chatId, T message) where T : class;

    Task DisconnectFromHubAsync();
}
