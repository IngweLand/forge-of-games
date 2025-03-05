using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using Ingweland.Fog.Application.Core.Services;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services;

public class WikipediaService(HttpClient httpClient, IMemoryCache cache, IMapper mapper, ILogger<WikipediaService> logger)
    : IWikipediaService
{
    private const string BaseUrlTemplate = "https://{0}.wikipedia.org/api/rest_v1/page/summary/";

    public async Task<WikipediaResponseDto?> GetArticleAbstractAsync(string title, string language, CancellationToken ct = default)
    {
        if (cache.TryGetValue(CreateCacheKey(title, language), out WikipediaResponseDto? cachedResponse))
        {
            return cachedResponse;
        }

        try
        {
            var localizedBaseUrl = string.Format(BaseUrlTemplate, language);
            var response =
                await httpClient.GetFromJsonAsync<WikipediaResponse>(
                    $"{localizedBaseUrl}{Uri.EscapeDataString(title)}", cancellationToken: ct);

            if (response?.Extract == null)
            {
                logger.LogWarning("No data returned from Wikipedia API for title: {title}", title);
                return null;
            }

            var dto = mapper.Map<WikipediaResponseDto>(response);
            cache.Set(CreateCacheKey(title, language), dto, TimeSpan.FromDays(30));
            return dto;
        }
        catch (HttpRequestException e)
        {
            logger.LogWarning(e, "Failed to fetch article from Wikipedia API for title: {title}", title);
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Failed to parse Wikipedia API response for title: {title}", title);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to fetch article from Wikipedia API for title: {title}", title);
        }

        return null;
    }

    private static string CreateCacheKey(string title, string language)
    {
        return $"{nameof(WikipediaResponse)}_{title}_{language}";
    }
}