using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Shared.Helpers;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class ExpansionYValueResolver:IValueResolver<ExpansionDefinitionDTO, ExpansionBasicData, int>
{
    public int Resolve(ExpansionDefinitionDTO source, ExpansionBasicData destination, int destMember, ResolutionContext context)
    {
        var expansionSize = GetExpansionSize(HohStringParser.GetConcreteId(source.CityId));
        return -source.Y - expansionSize;
    }
    
    private int GetExpansionSize(string cityId)
    {
        return cityId switch
        {
            "City_Vikings" => 3,
            _ => 4,
        };
    }
}
