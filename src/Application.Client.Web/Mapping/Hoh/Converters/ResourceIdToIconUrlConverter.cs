using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class ResourceIdToIconUrlConverter(IHohResourceIconUrlProvider resourceIconUrlProvider)
    : IValueConverter<string, string>
{
    public string Convert(string sourceMember, ResolutionContext context)
    {
        return resourceIconUrlProvider.GetIconUrl(sourceMember);
    }
}
