using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

public interface IBuildingTypeIconUrlProvider
{
    string GetIcon(BuildingType buildingType);
}
