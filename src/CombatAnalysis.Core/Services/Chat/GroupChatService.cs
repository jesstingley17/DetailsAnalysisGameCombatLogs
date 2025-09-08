using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Exceptions;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Services;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Services.Chat;

internal class GroupChatService(IHttpClientHelper httpClientHelper, ILogger<GroupChatService> logger) : IGroupChatService
{
    private readonly IHttpClientHelper _httpClientHelper = httpClientHelper;
    private readonly ILogger<GroupChatService> _logger = logger;

    public async Task<IEnumerable<AppUserModel>> GetFreeUsersToInviteAsync(List<AppUserModel> users)
    {
        try
        {
            var response = await _httpClientHelper.GetAsync($"GroupChatUser", API.ChatApi, true);
            response.EnsureSuccessStatusCode();

            var groupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
            ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

            var freeUsersToInvite = users.Where(x => !groupChatUsers.Any(y => x.Id == y.AppUserId)).ToList();
            ArgumentNullException.ThrowIfNull(freeUsersToInvite, nameof(freeUsersToInvite));

            return freeUsersToInvite;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while loading free users to invite to chat");

            throw new ChatServiceException("Failed to load free users to invite to chat", ex);
        }
    }

    public async Task<IEnumerable<GroupChatUserModel>> LoadChatUsersByUserIdAsync(string accountId)
    {
        try
        {
            var response = await _httpClientHelper.GetAsync($"GroupChatUser/findByUserId/{accountId}", API.ChatApi, true);
            response.EnsureSuccessStatusCode();

            var myGroupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
            ArgumentNullException.ThrowIfNull(myGroupChatUsers, nameof(myGroupChatUsers));

            return myGroupChatUsers;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while loading group chat users for AccountId={AccountId}", accountId);

            throw new ChatServiceException("Failed to load group chat users", ex);
        }
    }

    public async Task<IEnumerable<GroupChatModel>> LoadChatsAsync(IEnumerable<GroupChatUserModel> groupChatUsers)
    {
        try
        {
            var groupChats = new List<GroupChatModel>();
            foreach (var groupChatUser in groupChatUsers)
            {
                var response = await _httpClientHelper.GetAsync($"GroupChat/{groupChatUser.ChatId}", API.ChatApi, true);
                response.EnsureSuccessStatusCode();

                var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
                ArgumentNullException.ThrowIfNull(groupChat, nameof(groupChat));

                groupChats.Add(groupChat);
            }

            return groupChats;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while loading group chats");

            throw new ChatServiceException("Failed to load group chats", ex);
        }
    }

    public async Task<IEnumerable<GroupChatMessageModel>> LoadMessagesAsync(int chatId, string groupChatUserId)
    {
        try
        {
            var response = await _httpClientHelper.GetAsync($"GroupChatMessage/getByChatId?chatId={chatId}&groupChatUserId={groupChatUserId}&pageSize=20", API.ChatApi, true);
            response.EnsureSuccessStatusCode();

            var messages = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>();
            ArgumentNullException.ThrowIfNull(messages, nameof(messages));

            return messages;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while receiving group chat messages for ChatId={ChatId}", chatId);

            throw new ChatServiceException("Failed to receive group chat messages", ex);
        }
    }

    public async Task<IEnumerable<UnreadGroupChatMessageModel>> LoadUnreadMessagesAsync(int messageId)
    {
        try
        {
            var response = await _httpClientHelper.GetAsync($"UnreadGroupChatMessage/findByMessageId/{messageId}", API.ChatApi, true);
            response.EnsureSuccessStatusCode();

            var unreadGroupChatMessages = await response.Content.ReadFromJsonAsync<IEnumerable<UnreadGroupChatMessageModel>>();
            ArgumentNullException.ThrowIfNull(unreadGroupChatMessages, nameof(unreadGroupChatMessages));

            return unreadGroupChatMessages;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while receiving group chat unread messages for MessageId={MessageId}", messageId);

            throw new ChatServiceException("Failed to receive group chat unread messages", ex);
        }
    }

    public async Task InviteToChatAsync(int chatId, string userId)
    {
        try
        {
            var groupChatUser = new GroupChatUserModel
            {
                ChatId = chatId,
                AppUserId = userId,
            };

            var response = await _httpClientHelper.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser), API.ChatApi, true);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while inviting to group chat for ChatId={ChatId}", chatId);

            throw new ChatServiceException("Failed to invite to group chat", ex);
        }
    }

    public async Task EditChatMessageAsync(GroupChatMessageModel message)
    {
        try
        {
            var response = await _httpClientHelper.PutAsync("GroupChatMessage", JsonContent.Create(message), API.ChatApi, true);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while editing group chat message for MessageId={MessageId}", message.Id);

            throw new ChatServiceException("Failed to edit group chat message", ex);
        }
    }

    public async Task RemoveMessageAsync(int messageId)
    {
        try
        {
            var response = await _httpClientHelper.DeletAsync($"GroupChatMessage/{messageId}", API.ChatApi, true);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while removing group chat message for MessageId={MessageId}", messageId);

            throw new ChatServiceException("Failed to remove group chat message", ex);
        }
    }

    public async Task<GroupChatUserModel> GetUserInGroupChatAsync(int chatId, string accountId)
    {
        try
        {
            var response = await _httpClientHelper.GetAsync($"GroupChatUser/findMeInChat?chatId={chatId}&appUserId={accountId}", API.ChatApi, true);
            response.EnsureSuccessStatusCode();

            var userInChat = await response.Content.ReadFromJsonAsync<GroupChatUserModel>();
            ArgumentNullException.ThrowIfNull(userInChat, nameof(userInChat));

            return userInChat;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while getting user in group chat for AccountId={AccountId}", accountId);

            throw new ChatServiceException("Failed to get user in group chat", ex);
        }
    }
}
