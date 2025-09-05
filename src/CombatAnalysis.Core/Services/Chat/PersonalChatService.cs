using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Exceptions;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Services;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Services.Chat;

internal class PersonalChatService(IMemoryCache memoryCache, IHttpClientHelper httpClientHelper, ILogger<PersonalChatService> logger) : IPersonalChatService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IHttpClientHelper _httpClientHelper = httpClientHelper;
    private readonly ILogger<PersonalChatService> _logger = logger;

    public async Task<IEnumerable<PersonalChatMessageModel>> LoadMessagesAsync(int chatId)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

        try
        {
            var response = await _httpClientHelper.GetAsync($"PersonalChatMessage/getByChatId?chatId={chatId}&pageSize=20", refreshToken, API.ChatApi);
            response.EnsureSuccessStatusCode();

            var messages = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>();
            ArgumentNullException.ThrowIfNull(messages, nameof(messages));

            return messages;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while loading messages for personal chat");

            throw new ChatServiceException("Failed to load load messages for personal chat", ex);
        }
    }

    public async Task CreateNewPersonalChatAsync(string accountId, string companionId)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

        try
        {
            var personalChat = new PersonalChatModel
            {
                InitiatorId = accountId,
                CompanionId = companionId,
            };

            var response = await _httpClientHelper.PostAsync("PersonalChat", JsonContent.Create(personalChat), refreshToken, API.ChatApi);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while creating personal chat for AccountId={AccountId}", accountId);

            throw new ChatServiceException("Failed to create personal chat", ex);
        }
    }

    public async Task<IEnumerable<PersonalChatModel>> LoadPersonalChatsAsync(string accountId)
    {
        var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
        ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

        try
        {
            var response = await _httpClientHelper.GetAsync("PersonalChat", refreshToken, API.ChatApi);
            response.EnsureSuccessStatusCode();

            var personalChats = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>();
            var myPersonalChats = personalChats?
                .Where(x => x.InitiatorId == accountId || x.CompanionId == accountId)
                .ToList();
            ArgumentNullException.ThrowIfNull(myPersonalChats, nameof(myPersonalChats));

            await UpdatePersonalChatsAsync(myPersonalChats, accountId, refreshToken);

            return myPersonalChats;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while loading personal chats for AccountId={AccountId}", accountId);

            throw new ChatServiceException("Failed to load personal chats", ex);
        }
    }

    private async Task UpdatePersonalChatsAsync(IEnumerable<PersonalChatModel> myPersonalChats, string accountId, string refreshToken)
    {
        foreach (var chat in myPersonalChats)
        {
            var companionId = chat.CompanionId == accountId ? chat.InitiatorId : chat.CompanionId;
            var response = await _httpClientHelper.GetAsync($"Account/{companionId}", refreshToken, API.UserApi);
            response.EnsureSuccessStatusCode();

            var companion = await response.Content.ReadFromJsonAsync<AppUserModel>();
            ArgumentNullException.ThrowIfNull(companion, nameof(companion));

            chat.Username = companion?.Username ?? string.Empty;
        }
    }
}
