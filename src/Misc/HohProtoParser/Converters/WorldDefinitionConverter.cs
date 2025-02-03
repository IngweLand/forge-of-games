using AutoMapper;
using HohProtoParser.Extensions;
using HohProtoParser.Helpers;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class WorldDefinitionConverter :ITypeConverter<WorldDefinitionDTO, World>
{
    public World Convert(WorldDefinitionDTO source, World destination, ResolutionContext context)
    {
        var continents = (IList<Continent>)context.Items[ContextKeys.CONTINENTS];
        var continentsIds = source.Continents.Select(StringParser.ParseEnumFromString<ContinentId>);
        return new World()
        {
            Id = StringParser.ParseEnumFromString<WorldId>(source.World),
            Type = source.WorldType.ToWorldType(),
            Continents = continents.Where(c => continentsIds.Contains(c.Id)).ToList().AsReadOnly(),
        };
    }
}
