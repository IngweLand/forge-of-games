using AutoMapper;
using Ingweland.Fog.Application.Server.Helpers;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh.Converters;

public class EquipmentRarityValueConverter :IValueConverter<string, EquipmentRarity>
{
    public EquipmentRarity Convert(string sourceMember, ResolutionContext context)
    {
        var starCount = StringParser.GetConcreteId(sourceMember);
        
        return starCount switch
        {
            "3" => EquipmentRarity.Star_3,
            "4" => EquipmentRarity.Star_4,
            "5" => EquipmentRarity.Star_5,
            _ => EquipmentRarity.Undefined,
        };
    }
}