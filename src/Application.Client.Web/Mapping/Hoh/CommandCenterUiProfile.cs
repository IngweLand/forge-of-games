using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CommandCenter;
using Ingweland.Fog.Application.Client.Web.CommandCenter.Models;
using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh;

public class CommandCenterUiProfile : Profile
{
    public CommandCenterUiProfile()
    {
        CreateMap<BasicCommandCenterProfile, BasicCommandCenterProfile>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString("N")));
        CreateMap<CommandCenterProfile, BasicCommandCenterProfile>()
            .ForMember(dest => dest.Heroes, opt => opt.MapFrom(src => src.Heroes.Values.Select(x => x.Identifier)))
            .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.Teams.Values))
            .ForMember(dest => dest.SchemaVersion, opt => opt.MapFrom(src => FogConstants.CC_UI_PROFILE_SCHEME_VERSION));
        CreateMap<BasicCommandCenterProfile, CommandCenterProfile>()
            .ForMember(dest => dest.Heroes, opt => opt.Ignore());
        CreateMap<CommandCenterProfile, CcProfileBasicsViewModel>();
        CreateMap<BasicCommandCenterProfile, CcProfileBasicsViewModel>();
        CreateMap<IEnumerable<BasicCommandCenterProfile>, IDictionary<string, CcProfileBasicsViewModel>>()
            .ConvertUsing((src, _, context) =>
                src.ToDictionary(p => p.Id, p => context.Mapper.Map<CcProfileBasicsViewModel>(p)));
        CreateMap<IEnumerable<CommandCenterProfileTeam>, IDictionary<string, CommandCenterProfileTeam>>()
            .ConvertUsing(src => src.ToDictionary(p => p.Id));
        CreateMap<CommandCenterProfile, CcProfileSettings>();
    }
}
