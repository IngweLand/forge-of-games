using AutoMapper;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class RelicRarityValueConverter : IValueConverter<string, RelicRarity>
{
    public RelicRarity Convert(string sourceMember, ResolutionContext context)
    {
        var starCount = HohStringParser.GetConcreteId(sourceMember);

        return starCount switch
        {
            "4" => RelicRarity.Star_4,
            "5" => RelicRarity.Star_5,
            _ => RelicRarity.Undefined,
        };
    }
}