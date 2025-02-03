using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Calculators.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class HeroProgressionCostResourceToResourceAmountsConverter(
    IHeroProgressionCalculators heroProgressionCalculators,
    IHohResourceIconUrlProvider resourceIconUrlProvider)
    : ITypeConverter<HeroProgressionCostResource, IReadOnlyCollection<IconLabelItemViewModel>>
{
    public IReadOnlyCollection<IconLabelItemViewModel> Convert(HeroProgressionCostResource source,
        IReadOnlyCollection<IconLabelItemViewModel> destination,
        ResolutionContext context)
    {
        return new List<IconLabelItemViewModel>
        {
            new()
            {
                IconUrl = resourceIconUrlProvider.GetIconUrl("resource.hero_xp"),
                Label = source.Amount.ToString("N0"),
            },
            new()
            {
                IconUrl = resourceIconUrlProvider.GetIconUrl(source.ResourceId),
                Label = heroProgressionCalculators.CalculateDependentCost(source).ToString("N0"),
            },
        }.AsReadOnly();
    }
}
