using CombatAnalysis.Core.Models.Chat;

namespace CombatAnalysis.Core.Interfaces;

public interface IChatHubHelper
{
    Task ConnectToChatHubAsync(string hubURL);

    Task JoinChatRoomAsync(string appUserId);

    Task ConnectToChatMessagesHubAsync(string hubURL);

    Task JoinChatMessagesRoomAsync(int chatId);

    Task SendMessageAsync(string message, int chatId, string appUserId, string username, int type = -1);

    Task ConnectToUnreadMessagesHubAsync(string hubURL);

    Task JoinUnreadMessagesRoomAsync(int chatId);

    void SubscribeToChat(Action<PersonalChatModel> callback);

    void SubscribeUnreadMessagesUpdated(Action<int, string, int> receiveUnreadMessageAction);

    void SubscribeMessagesUpdated<T>(int chatId, string meInChatId, Action<T> action) where T : class;

    Task SubscribeMessageHasBeenReadAsync(int messageId, string appUserId);

    void SubscribeReceiveMessageHasBeenRead<T>(Action<T> action);

    Task LeaveFromChatRoomAsync(int chatId);

    Task LeaveFromUnreadMessageRoomAsync(int chatId);

    Task StopAsync();
}
