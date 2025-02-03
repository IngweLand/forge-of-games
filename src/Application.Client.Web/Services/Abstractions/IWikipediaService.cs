using Ingweland.Fog.Application.Client.Web.Models;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IWikipediaService
{
    Task<WikipediaResponse?> GetArticleAbstractAsync(string title, string language);
}
