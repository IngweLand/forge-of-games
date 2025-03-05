using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Refit;

namespace Ingweland.Fog.Application.Core.Services;

public interface IWikipediaService
{
    [Get(FogUrlBuilder.ApiRoutes.WIKI_EXTRACT)]
    Task<WikipediaResponseDto?> GetArticleAbstractAsync([Query] string title, [Query] string language,
        CancellationToken ct = default);
}