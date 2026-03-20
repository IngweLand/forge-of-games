using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Dtos.Hoh.Units;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class HeroAbilityService(ISender sender) : IHeroAbilityService
{
    public Task<IReadOnlyCollection<HeroAbilityFeaturesDto>> GetHeroAbilityFeaturesAsync()
    {
        var query = new GetHeroAbilityFeaturesQuery();
        return sender.Send(query);
    }
}
