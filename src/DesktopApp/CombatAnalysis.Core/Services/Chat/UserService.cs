using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Exceptions;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Interfaces.Services;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Services.Chat;

internal class UserService(IHttpClientHelper httpClientHelper, ILogger<UserService> logger) : IUserService
{
    private readonly IHttpClientHelper _httpClientHelper = httpClientHelper;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<IEnumerable<AppUserModel>> LoadUsersAsync()
    {
        try
        {
            var response = await _httpClientHelper.GetAsync("User", API.UserApi, true);
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<IEnumerable<AppUserModel>>();
            ArgumentNullException.ThrowIfNull(users, nameof(users));

            return users;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error while loading users");

            throw new ChatServiceException("Failed to load users", ex);
        }
    }
}
