using AutoMapper;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.Mapping.Hoh;

public class FogCommonMappingProfile : Profile
{
    public FogCommonMappingProfile()
    {
        CreateMap<AnnualBudget, AnnualBudgetDto>()
            .ForMember(dest => dest.ServerGoalCompletion,
                opt => opt.MapFrom(x =>
                    (float) Math.Round(x.ServerContributions / x.ServerGoal, 3, MidpointRounding.ToZero)));

        CreateMap<CommunityCityStrategyEntity, CommunityCityStrategyDto>();
    }
}
