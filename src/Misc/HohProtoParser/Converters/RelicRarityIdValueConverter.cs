using AutoMapper;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.HohProtoParser.Converters;

public class RelicRarityIdValueConverter:IValueConverter<string, RelicRarityId>
{
    public RelicRarityId Convert(string sourceMember, ResolutionContext context)
    {
        return sourceMember switch
        {
            "relic_rarity.2" => RelicRarityId.Rarity_2,
            "relic_rarity.3" => RelicRarityId.Rarity_3,
            "relic_rarity.4" => RelicRarityId.Rarity_4,
            "relic_rarity.5" => RelicRarityId.Rarity_5,
            _ => throw new InvalidOperationException($"Unhandled relic rarity id: {sourceMember}"),
        };
    }
}
