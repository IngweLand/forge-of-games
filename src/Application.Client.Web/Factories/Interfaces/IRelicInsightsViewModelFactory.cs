using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Relic;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IRelicInsightsViewModelFactory
{
    RelicInsightsViewModel Create(RelicInsightsDto dto, IReadOnlyDictionary<string, RelicDto> relics);
}
