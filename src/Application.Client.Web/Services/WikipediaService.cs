using System.Net.Http.Json;
using System.Text.Json;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.Services;

public class WikipediaService(HttpClient httpClient, IMemoryCache cache, ILogger<WikipediaService> logger)
    : IWikipediaService
{
    private const string CACHE_KEY_PREFIX = nameof(WikipediaResponse);
    private string _baseAddressFormat = "https://{0}.wikipedia.org/api/rest_v1/page/summary/";

    public async Task<WikipediaResponse?> GetArticleAbstractAsync(string title, string language)
    {
        if (cache.TryGetValue($"{CACHE_KEY_PREFIX}_{title}", out WikipediaResponse? cachedResponse))
        {
            return cachedResponse;
        }

        try
        {
            var localizedBaseUrl = string.Format(_baseAddressFormat, language);
            var response =
                await httpClient.GetFromJsonAsync<WikipediaResponse>(
                    $"{localizedBaseUrl}{Uri.EscapeDataString(title)}");

            if (response == null)
            {
                logger.LogWarning($"No data returned from Wikipedia API for title: {title}");
                return null;
            }

            cache.Set($"{CACHE_KEY_PREFIX}_{title}", response, TimeSpan.FromDays(1));
            return response;
        }
        catch (HttpRequestException e)
        {
            logger.LogWarning(e, $"Failed to fetch article from Wikipedia API for title: {title}");
        }
        catch (JsonException e)
        {
            logger.LogError(e, $"Failed to parse Wikipedia API response for title: {title}");
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to fetch article from Wikipedia API for title: {title}");
        }

        return null;
    }
}
