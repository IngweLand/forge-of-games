using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class ProductionStatsViewModelFactory(IHohResourceIconUrlProvider resourceIconUrlProvider)
    : IProductionStatsViewModelFactory
{
    public ProductionStatsViewModel Create(IDictionary<string, ConsolidatedTimedProductionValues> products,
        IDictionary<string, ConsolidatedTimedProductionValues> costs)
    {
        var productsViewModels = products.Select(x => new TimedProductionValuesViewModel
            {
                Default = x.Value.Default.ToString("N0"),
                OneHour = x.Value.OneHour.ToString("N0"),
                OneDay = x.Value.OneDay.ToString("N0"),
                IconUrl = resourceIconUrlProvider.GetIconUrl(x.Key),
            })
            .ToList();

        var costsViewModels = costs.Select(x => new TimedProductionValuesViewModel
            {
                Default = x.Value.Default.ToString("N0"),
                OneHour = x.Value.OneHour.ToString("N0"),
                OneDay = x.Value.OneDay.ToString("N0"), 
                IconUrl = resourceIconUrlProvider.GetIconUrl(x.Key),
            })
            .ToList();

        return new ProductionStatsViewModel
        {
            Products = productsViewModels,
            Costs = costsViewModels,
        };
    }
}
