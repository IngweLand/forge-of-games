using ResourceType = Ingweland.Fog.Models.Hoh.Enums.ResourceType;

namespace Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

public interface IHohResourceIconUrlProvider
{
    string GetIconUrl(string resourceId);
    string GetIconUrl(ResourceType resourceType);
}
