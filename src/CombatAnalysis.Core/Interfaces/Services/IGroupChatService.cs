using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;

namespace CombatAnalysis.Core.Interfaces.Services;

public interface IGroupChatService
{
    Task<IEnumerable<AppUserModel>> GetFreeUsersToInviteAsync(List<AppUserModel> users);

    Task<IEnumerable<GroupChatUserModel>> LoadChatUsersByUserIdAsync(string accountId);

    Task<IEnumerable<GroupChatModel>> LoadChatsAsync(IEnumerable<GroupChatUserModel> groupChatUsers);

    Task<IEnumerable<GroupChatMessageModel>> LoadMessagesAsync(int chatId, string groupChatUserId);

    Task<IEnumerable<UnreadGroupChatMessageModel>> LoadUnreadMessagesAsync(int messageId);

    Task InviteToChatAsync(int chatId, string userId);

    Task EditChatMessageAsync(GroupChatMessageModel message);

    Task RemoveMessageAsync(int messageId);

    Task<GroupChatUserModel> GetUserInGroupChatAsync(int chatId, string accountId);
}
