using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;
using Markdig;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class CommunityCityStrategyViewModelFactory(IAssetUrlProvider assetUrlProvider)
    : ICommunityCityStrategyViewModelFactory
{
    public CommunityCityStrategyViewModel Create(CommunityCityStrategyDto dto,
        IReadOnlyDictionary<string, AgeViewModel> ages)
    {
        string? cityIconUrl = null;
        if (dto.WonderId != null && dto.WonderId != WonderId.Undefined)
        {
            cityIconUrl = assetUrlProvider.GetHohIconUrl(dto.CityId.GetIcon());
        }

        AgeViewModel? age = null;
        if (dto.AgeId != null && ages.TryGetValue(dto.AgeId, out var foundAge))
        {
            age = foundAge;
        }

        return new CommunityCityStrategyViewModel
        {
            SharedDataId = dto.SharedDataId,
            Author = dto.Author,
            CityId = dto.CityId,
            UpdatedAt = dto.UpdatedAt.ToString("g"),
            Age = age,
            Name = dto.Name,
            CityIconUrl = cityIconUrl,
        };
    }

    public CommunityCityGuideViewModel Create(CommunityCityGuideInfoDto infoDto,
        IReadOnlyDictionary<string, AgeViewModel> ages)
    {
        string? cityIconUrl = null;
        if (infoDto.WonderId != null && infoDto.WonderId != WonderId.Undefined)
        {
            cityIconUrl = assetUrlProvider.GetHohIconUrl(infoDto.CityId.GetIcon());
        }

        AgeViewModel? age = null;
        if (infoDto.AgeId != null && ages.TryGetValue(infoDto.AgeId, out var foundAge))
        {
            age = foundAge;
        }

        return new CommunityCityGuideViewModel
        {
            Id = infoDto.Id,
            Author = infoDto.Author,
            CityId = infoDto.CityId,
            UpdatedAt = infoDto.UpdatedAt.ToString("g"),
            Age = age,
            Name = infoDto.Name,
            CityIconUrl = cityIconUrl,
        };
    }

    public CommunityCityGuideViewModel Create(CommunityCityGuideDto dto, AgeViewModel? age)
    {
        string? cityIconUrl = null;
        if (dto.WonderId != null && dto.WonderId != WonderId.Undefined)
        {
            cityIconUrl = assetUrlProvider.GetHohIconUrl(dto.CityId.GetIcon());
        }

        var htmlContent = string.Empty;
        try
        {
            htmlContent = Markdown.ToHtml(dto.Content);
        }
        catch (Exception e)
        {
            Console.Out.WriteLine(e);
        }

        return new CommunityCityGuideViewModel
        {
            Id = dto.Id,
            Author = dto.Author,
            CityId = dto.CityId,
            UpdatedAt = dto.UpdatedAt.ToString("g"),
            Age = age,
            Name = dto.Name,
            CityIconUrl = cityIconUrl,
            Content = htmlContent,
        };
    }
}
