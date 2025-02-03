using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

public interface IHohStorageIconUrlProvider
{
    string GetIconUrl(BuildingType buildingType);
    string GetIconUrl(string resourceId);
}
