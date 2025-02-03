using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class ProductionStatsViewModelFactory(IHohResourceIconUrlProvider resourceIconUrlProvider) : IProductionStatsViewModelFactory
{
    public ProductionStatsViewModel Create(IDictionary<string, ConsolidatedCityProduct> products)
    {
        var productsViewModels = new List<CityProductViewModel>();
        foreach (var product in products)
        {
            productsViewModels.Add(new CityProductViewModel()
            {
                DefaultProduction = product.Value.Default.ToString("N0"),
                OneHourProduction = product.Value.OneHour.ToString("N0"),
                OneDayProduction = product.Value.OneDay.ToString("N0"),
                IconUrl = resourceIconUrlProvider.GetIconUrl(product.Key),
            });
        }

        return new ProductionStatsViewModel()
        {
            Products = productsViewModels,
        };
    }
}
