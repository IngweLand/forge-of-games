using AutoMapper;
using HohProtoParser.Extensions;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;

namespace HohProtoParser.Converters;

public class WorldDefinitionConverter : ITypeConverter<WorldDefinitionDTO, World>
{
    public World Convert(WorldDefinitionDTO source, World destination, ResolutionContext context)
    {
        var continents = (IList<Continent>) context.Items[ContextKeys.CONTINENTS];
        var continentsIds = source.Continents.Select(HohStringParser.ParseEnumFromString<ContinentId>);
        return new World()
        {
            Id = HohStringParser.ParseEnumFromString<WorldId>(source.World),
            Type = source.WorldType.ToWorldType(),
            Continents = continents.Where(c => continentsIds.Contains(c.Id)).ToList().AsReadOnly(),
        };
    }
}
