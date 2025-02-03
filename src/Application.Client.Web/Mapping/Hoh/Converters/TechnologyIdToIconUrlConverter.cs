using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class TechnologyIdToIconUrlConverter(IAssetUrlProvider assetUrlProvider)
    : IValueConverter<string, string>
{
    public string Convert(string sourceMember, ResolutionContext context)
    {
        return assetUrlProvider.GetHohTechnologyImageUrl(sourceMember);
    }
}
