using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class CityIdToExpansionIconUrlConverter(IAssetUrlProvider assetUrlProvider) : IValueConverter<CityId, string>
{
    public string Convert(CityId sourceMember, ResolutionContext context)
    {
        var id = sourceMember switch
        {
            CityId.China => $"icon_expansion_city_china_land",
            CityId.Egypt => $"icon_expansion_city_egypt_land",
            CityId.Vikings => $"icon_expansion_city_vikings_land",
            _ => $"icon_expansion_city_capital_land",
        };

        return assetUrlProvider.GetHohIconUrl(id);
    }
}
