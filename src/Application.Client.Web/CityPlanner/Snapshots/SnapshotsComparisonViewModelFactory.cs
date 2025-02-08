using Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots.Abstractions;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Snapshots;

public class SnapshotsComparisonViewModelFactory(IHohResourceIconUrlProvider resourceIconUrlProvider)
    : ISnapshotsComparisonViewModelFactory
{
    public SnapshotsComparisonViewModel Create(IReadOnlyDictionary<HohCitySnapshot, CityStats> stats)
    {
        var columns = stats.Keys.ToList();
        return new SnapshotsComparisonViewModel()
        {
            SnapshotNames = columns.Select(src => src.ComputedName).ToList(),
            Production = CreateProductionItems(columns, stats),
        };
    }

    private List<SnapshotsComparisonRow> CreateProductionItems(List<HohCitySnapshot> columns,
        IReadOnlyDictionary<HohCitySnapshot, CityStats> stats)
    {
        var rows = new List<SnapshotsComparisonRow>();
        var allProducts = stats.Values.SelectMany(src => src.Products.Keys).ToHashSet();
        foreach (var productKey in allProducts)
        {
            var values = columns.Select(column =>
                stats[column].Products.TryGetValue(productKey, out var value)
                    ? value.Default
                    : 0).ToList();
            var maxValue = values.Max();
            var different = values.Any(i => i != maxValue);

            var cells = values.Select(i => new SnapshotsComparisonCellValue()
            {
                Value = i != 0 ? i.ToString("N0") : "-",
                IsLargest = different && i == maxValue,
            }).ToList();

            var row = new SnapshotsComparisonRow()
            {
                Header = resourceIconUrlProvider.GetIconUrl(productKey),
                Cells = cells,
            };
            rows.Add(row);
        }

        return rows;
    }
}
