using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class AllianceMemberRoleToIconUrlConverter(IAllianceMemberRoleIconUrlProvider iconUrlProvider)
    : IValueConverter<AllianceMemberRole, string?>
{
    public string? Convert(AllianceMemberRole sourceMember, ResolutionContext context)
    {
        return iconUrlProvider.GetIconUrl(sourceMember);
    }
}
