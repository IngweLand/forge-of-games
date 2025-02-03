using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace HohProtoParser.Converters;

public class WonderLevelCostsConverter : ITypeConverter<WonderLevelUpComponentDTO, WonderLevelUpComponent>
{
    public WonderLevelUpComponent Convert(WonderLevelUpComponentDTO source, WonderLevelUpComponent destination,
        ResolutionContext context)
    {
        var costs = source.UpgradeCosts.Costs.OrderBy(wuc => wuc.Level)
            .Select(levelData => new WonderLevelCost
        {
            Level = levelData.Level,
            Crates = context.Mapper.Map<IReadOnlyCollection<WonderCrate>>(levelData.CratesWrapper.Crates),
        }).ToList();

        return new WonderLevelUpComponent
        {
            LevelCosts = costs,
        };
    }
}
