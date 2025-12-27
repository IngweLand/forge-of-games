using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class ProductionStatsViewModelFactory(IHohResourceIconUrlProvider resourceIconUrlProvider)
    : IProductionStatsViewModelFactory
{
    private static readonly PriorityKeyComparer KeyComparer = new();

    public ProductionStatsViewModel Create(IDictionary<string, ConsolidatedTimedProductionValues> products,
        IDictionary<string, ConsolidatedTimedProductionValues> costs)
    {
        var productsViewModels = products
            .OrderBy(x => x.Key, KeyComparer)
            .Select(x => new TimedProductionValuesViewModel
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

    private class PriorityKeyComparer : IComparer<string>
    {
        private static readonly string[] PriorityKeys =
        [
            "resource.coins", "resource.food", // capital
            "resource.dirham", "resource.gold_fal", "resource.coffee_beans", "resource.brass", "resource.myrrh",
            "resource.oil", // arabia
            "resource.wu_zhu", "resource.rice", // china
            "resource.pennies", "resource.honey", "resource.fish", // vikings
            "resource.deben", "resource.papyrus", "resource.gold_ore", // egypt
            "resource.cocoa", "resource.jade", "resource.obsidian", "resource.feathers", // maya
        ];

        public int Compare(string? x, string? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            var indexX = Array.IndexOf(PriorityKeys, x);
            var indexY = Array.IndexOf(PriorityKeys, y);

            // If both are in the priority list, sort by their position in that list
            if (indexX != -1 && indexY != -1)
            {
                return indexX.CompareTo(indexY);
            }

            // If only one is in the priority list, it goes first
            if (indexX != -1)
            {
                return -1;
            }

            if (indexY != -1)
            {
                return 1;
            }

            // Otherwise, sort naturally (alphabetically)
            return string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
        }
    }
}
