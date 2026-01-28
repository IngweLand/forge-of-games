using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Hoh.Enums;

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
            GuideId = dto.GuideId,
        };
    }
}
