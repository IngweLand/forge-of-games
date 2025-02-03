using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class BuildingGroupDtoToViewModelConverter(IAssetUrlProvider assetUrlProvider, IBuildingViewModelFactory buildingViewModelFactory)
    : ITypeConverter<BuildingGroupDto, BuildingGroupViewModel>
{
    public BuildingGroupViewModel Convert(BuildingGroupDto source, BuildingGroupViewModel destination,
        ResolutionContext context)
    {
        return new BuildingGroupViewModel()
        {
            Id = source.Id,
            Name = source.Name,
            TypeName = source.TypeName,
            TypeIconUrl = assetUrlProvider.GetHohIconUrl(source.Type.GetBuildingTypeIconId()),
            Buildings = source.Buildings.Select(b => buildingViewModelFactory.Create(b)).ToList(),
            BuildingSize = $"{source.Buildings.First().Width} x {source.Buildings.First().Length}",
            CityName = source.CityName,
        };
    }
}
