using Ingweland.Fog.Application.Client.Web.CommandCenter.Abstractions;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CommandCenter.Factories;

public class BarracksViewModelFactory(IBuildingLevelRangesFactory buildingLevelRangesFactory)
    : IBarracksViewModelFactory
{
    private static readonly IList<BuildingGroup> BarracksBuildingGroups = new List<BuildingGroup>()
    {
        BuildingGroup.InfantryBarracks, BuildingGroup.CavalryBarracks, BuildingGroup.RangedBarracks,
        BuildingGroup.SiegeBarracks, BuildingGroup.HeavyInfantryBarracks,
    };

    public IReadOnlyDictionary<BuildingGroup, CcBarracksViewModel> Create(IReadOnlyCollection<BuildingDto> buildings)
    {
        var ranges = buildingLevelRangesFactory.Create(buildings);
        var vms = new Dictionary<BuildingGroup, CcBarracksViewModel>();
        foreach (var barracksBuildingGroup in BarracksBuildingGroups)
        {
            var barracksRanges = ranges[barracksBuildingGroup];
            var barracksLevels = Enumerable.Range(barracksRanges.StartLevel,
                barracksRanges.EndLevel - barracksRanges.StartLevel + 1).ToList();
            vms.Add(barracksBuildingGroup, new CcBarracksViewModel()
            {
                Group = barracksBuildingGroup,
                Name = buildings.First(b => b.Group == barracksBuildingGroup).Name,
                Levels = barracksLevels,
            });
        }

        return vms;
    }
}
