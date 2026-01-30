using FluentResults;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityStrategyBuilder.Abstractions;

public interface ICityGuideMarkdownConverter
{
    Task<Result<string>> Convert(string markdown, WonderId wonderId, string fogBaseUrl,
        IProgress<string>? progress = null);
}
