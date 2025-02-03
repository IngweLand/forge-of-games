using AutoMapper;
using HohProtoParser.Helpers;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class RegionDefinitionConverter :ITypeConverter<RegionDefinitionDTO, Region>
{
    public Region Convert(RegionDefinitionDTO source, Region destination, ResolutionContext context)
    {
        var continents = (IList<ContinentDefinitionDTO>)context.Items[ContextKeys.CONTINENT_DEFINITIONS];
        var encounters = (IList<Encounter>)context.Items[ContextKeys.ENCOUNTERS];
        var index = continents.Single(cd => cd.Continent == source.Continent).Regions.IndexOf(source.Region);
        return new Region()
        {
            Id = StringParser.ParseEnumFromString<RegionId>(source.Region),
            Index = index,
            Reward = context.Mapper.Map<RegionReward>(source.Reward),
            Encounters = encounters.Where(e => source.Encounters.Contains(e.Id)).ToList().AsReadOnly(),
        };
    }
}
