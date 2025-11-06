using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class RegionDefinitionConverter : ITypeConverter<RegionDefinitionDTO, Region>
{
    public Region Convert(RegionDefinitionDTO source, Region destination, ResolutionContext context)
    {
        var continents = (IList<ContinentDefinitionDTO>) context.Items[ContextKeys.CONTINENT_DEFINITIONS];
        var encounters = (IList<Encounter>) context.Items[ContextKeys.ENCOUNTERS];
        var index = continents.Single(cd => cd.Continent == source.Continent).Regions.IndexOf(source.Region);
        return new Region()
        {
            Id = HohStringParser.ParseEnumFromString<RegionId>(source.Region),
            Index = index,
            Rewards = context.Mapper.Map<IReadOnlyDictionary<Difficulty, RegionReward>>(source.Rewards),
            Encounters = encounters.Where(e => source.Encounters.Contains(e.Id)).ToList().AsReadOnly(),
        };
    }
}
