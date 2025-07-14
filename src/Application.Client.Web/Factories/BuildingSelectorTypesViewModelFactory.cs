using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BuildingSelectorTypesViewModelFactory(IMapper mapper, IAssetUrlProvider assetUrlProvider)
    : IBuildingSelectorTypesViewModelFactory
{
    public BuildingSelectorTypesViewModel Create(BuildingType buildingType, IEnumerable<BuildingDto> buildings)
    {
        var groups = buildings.DistinctBy(b => b.Group);
        return new BuildingSelectorTypesViewModel
        {
            Icon = GetIconString(assetUrlProvider.GetHohIconUrl(buildingType.GetBuildingTypeIconId())),
            BuildingGroups = mapper.Map<IReadOnlyCollection<BuildingSelectorItemViewModel>>(groups),
        };
    }

    private string GetIconString(string icon)
    {
        return $"<image width=\"100%\" height=\"100%\" xlink:href=\"{icon}\" preserveAspectRatio=\"xMidYMid meet\"/>";
    }
}
