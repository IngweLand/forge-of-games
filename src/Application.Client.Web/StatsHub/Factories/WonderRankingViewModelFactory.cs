using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class WonderRankingViewModelFactory(IAssetUrlProvider assetUrlProvider) : IWonderRankingViewModelFactory
{
    public WonderRankingViewModel Create(WonderRankingDto dto)
    {
        return new WonderRankingViewModel
        {
            EventLabel = $"{dto.StartedAt:d} - {dto.EndedAt:d}",
            Level = dto.Level.ToString(),
            WonderName = dto.WonderName,
            WonderIconUrl = assetUrlProvider.GetHohIconUrl(dto.Wonder.ToCity().GetIcon()),
        };
    }
}
