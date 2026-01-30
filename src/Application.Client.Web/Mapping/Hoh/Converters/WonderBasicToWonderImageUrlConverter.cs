using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Core.Extensions;
using Ingweland.Fog.Dtos.Hoh.City;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class WonderBasicToWonderImageUrlConverter(IAssetUrlProvider assetUrlProvider)
    : IValueConverter<WonderBasicDto, string>
{
    public string Convert(WonderBasicDto sourceMember, ResolutionContext context)
    {
        return assetUrlProvider.GetHohImageUrl(sourceMember.Id.GetImageFileName());
    }
}
