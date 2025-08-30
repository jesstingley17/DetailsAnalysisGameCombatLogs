using CombatAnalysis.ChatApi.Models;

namespace CombatAnalysis.ChatApi.Interfaces;

public interface IChatHubHelper
{
    Task ConnectToHubAsync(string hubURL, string refreshToken, string accessToken);

    Task JoinRoomAsync(int chatId);

    Task RequestUnreadMessagesAsync(int chatId, string appUserId);

    Task SendMessageAlreadyRead(int chatId, int chatMessageId);

    Task RequestsChats(int chatId, string appUserId);

    Task RequestsMessage(int chatId, GroupChatMessageModel message);
}
