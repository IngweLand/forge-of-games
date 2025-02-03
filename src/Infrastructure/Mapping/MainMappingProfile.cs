using AutoMapper;
using Ingweland.Fog.Infrastructure.Entities;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Infrastructure.Mapping;

public class MainMappingProfile:Profile
{
    public MainMappingProfile()
    {
        CreateMap<InGameStartupData, InGameStartupDataTableEntity>()
            .ForMember(dest => dest.CitiesJson, opt => opt.Ignore())
            .ForMember(dest => dest.ProfileJson, opt => opt.Ignore())
            .ForMember(dest => dest.RelicsJson, opt => opt.Ignore());
        CreateMap<InGameStartupDataTableEntity, InGameStartupData>();
    }
}
