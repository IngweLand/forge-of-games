using AutoMapper;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class ToolsUiProfile :Profile
{
    public ToolsUiProfile()
    {
        CreateMap<BuildingViewModel, BuildingLevelCostData>()
            .ForMember(dst => dst.ConstructionComponent,
                opt => opt.MapFrom(src => src.Data.Components.OfType<ConstructionComponent>().FirstOrDefault()))
            .ForMember(dst => dst.UpgradeComponent,
                opt => opt.MapFrom(src => src.Data.Components.OfType<UpgradeComponent>().FirstOrDefault()));
    }
}
