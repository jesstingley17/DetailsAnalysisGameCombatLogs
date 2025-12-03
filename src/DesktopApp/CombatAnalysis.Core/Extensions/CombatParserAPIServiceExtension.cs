using CombatAnalysis.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Extensions;

internal static class CombatParserAPIServiceExtension
{
    public static async Task<IEnumerable<T>> LoadCombatDetailsAsync<T>(this ICombatParserAPIService _, IHttpClientHelper httpClient, ILogger logger, string address, CancellationToken token)
        where T : class
    {
        try
        {
            var response = await httpClient.GetAsync(address, token);
            response.EnsureSuccessStatusCode();

            var details = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
            ArgumentNullException.ThrowIfNull(details, nameof(details));

            return details;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, ex.Message);

            return [];
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return [];
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Request was canceled by client: {Message}", ex.Message);

            return [];
        }
    }
}
