using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class AllianceAvatarBackgroundIdToUrlConverter(IAllianceAvatarUrlProvider allianceAvatarUrlProvider)
    : IValueConverter<int, string>
{
    public string Convert(int sourceMember, ResolutionContext context)
    {
        return allianceAvatarUrlProvider.GetBackgroundUrl(sourceMember);
    }
}