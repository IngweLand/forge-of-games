using AutoMapper;
using HohProtoParser.Helpers;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class ContinentDefinitionConverter :ITypeConverter<ContinentDefinitionDTO, Continent>
{
    public Continent Convert(ContinentDefinitionDTO source, Continent destination, ResolutionContext context)
    {
        var worlds = (IList<WorldDefinitionDTO>)context.Items[ContextKeys.WORLD_DEFINITIONS];
        var regions = (IList<Region>)context.Items[ContextKeys.REGIONS];
        var index = worlds.Single(cd => cd.World == source.World).Continents.IndexOf(source.Continent);
        var regionIds = source.Regions.Select(StringParser.ParseEnumFromString<RegionId>);
        return new Continent()
        {
            Id = StringParser.ParseEnumFromString<ContinentId>(source.Continent),
            Index = index,
            Regions = regions.Where(c => regionIds.Contains(c.Id)).ToList().AsReadOnly(),
        };
    }
}
