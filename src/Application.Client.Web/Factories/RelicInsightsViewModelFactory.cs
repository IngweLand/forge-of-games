using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Relic;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class RelicInsightsViewModelFactory(IAssetUrlProvider assetUrlProvider)
    : IRelicInsightsViewModelFactory
{
    public RelicInsightsViewModel Create(RelicInsightsDto dto, IReadOnlyDictionary<string, RelicDto> relics)
    {
        string levelRange;
        if (dto.FromLevel == 0)
        {
            levelRange = $"<{dto.ToLevel}";
        }
        else if (dto.ToLevel == int.MaxValue)
        {
            levelRange = $"{dto.FromLevel}+";
        }
        else
        {
            levelRange = $"{dto.FromLevel}-{dto.ToLevel}";
        }

        return new RelicInsightsViewModel
        {
            LevelRange = levelRange,
            Relics = dto.Relics.Where(relics.ContainsKey).Select(x =>
            {
                var relic = relics[x];
                return new IconLabelItemViewModel()
                {
                    Label = relic.Name,
                    IconUrl = assetUrlProvider.GetHohRelicIconUrl(HohStringParser.GetConcreteId(relic.Id)),
                };
            }).ToList(),
        };
    }
}
