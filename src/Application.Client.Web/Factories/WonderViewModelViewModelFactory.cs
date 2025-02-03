using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class WonderViewModelViewModelFactory(IMapper mapper, IAssetUrlProvider assetUrlProvider)
    : IWonderViewModelViewModelFactory
{
    public WonderViewModel Create(WonderDto wonderDto)
    {
        var levels = new List<WonderLevelViewModel>();
        var cumulativeCratesCount = 0;
        var cumulativeCost = new List<WonderCrate>();
        var crateIconUrl = assetUrlProvider.GetHohIconUrl("icon_crate");
        for (var index = 0; index < wonderDto.Levels.Count; index++)
        {
            var dtoLevel = wonderDto.Levels[index];
            var consolidatedCost = ConsolidateCrates(dtoLevel.Cost);
            cumulativeCost = AddCrates(cumulativeCost.Concat(consolidatedCost));
            var cratesCount = consolidatedCost.Sum(wc => wc.Amount);
            cumulativeCratesCount += cratesCount;
            var costViewModels = mapper.Map<IList<IconLabelItemViewModel>>(
                consolidatedCost.Select(wc => wc.FillResource));
            costViewModels.Insert(0, new IconLabelItemViewModel()
            {
                Label = cratesCount.ToString("N0"),
                IconUrl = crateIconUrl,
            });
            var cumulativeCostViewModels =
                mapper.Map<IList<IconLabelItemViewModel>>(
                    cumulativeCost.Select(wc => wc.FillResource));
            cumulativeCostViewModels.Insert(0, new IconLabelItemViewModel()
            {
                Label = cumulativeCratesCount.ToString("N0"),
                IconUrl = crateIconUrl,
            });
            levels.Add(new WonderLevelViewModel
            {
                Level = index + 1,
                Cost = costViewModels.AsReadOnly(),
                CumulativeCost = cumulativeCostViewModels.AsReadOnly(),
            });
        }

        return new WonderViewModel
        {
            Id = wonderDto.Id,
            Name = wonderDto.WonderName,
            CityName = wonderDto.CityName,
            Levels = levels,
            Data = wonderDto,
        };
    }

    private List<WonderCrate> AddCrates(IEnumerable<WonderCrate> src)
    {
        return src
            .GroupBy(obj => obj.FillResource.ResourceId)
            .Select(group => new WonderCrate
            {
                Amount = group.Sum(obj => obj.Amount),
                FillResource = new ResourceAmount
                {
                    ResourceId = group.Key,
                    Amount = group.Sum(obj => obj.FillResource.Amount),
                },
            })
            .ToList();
    }

    private List<WonderCrate> ConsolidateCrates(IEnumerable<WonderCrate> src)
    {
        return src
            .GroupBy(obj => obj.FillResource.ResourceId)
            .Select(group => new WonderCrate
            {
                Amount = group.Sum(obj => obj.Amount),
                FillResource = new ResourceAmount
                {
                    ResourceId = group.Key,
                    Amount = group.Sum(obj => obj.Amount * obj.FillResource.Amount),
                },
            })
            .ToList();
    }
}
