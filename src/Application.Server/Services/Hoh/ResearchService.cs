using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.Research;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class ResearchService(
    IHohCoreDataRepository hohCoreDataRepository,
    IMapper mapper,
    ILogger<TreasureHuntService> logger) : IResearchService
{
    public async Task<IReadOnlyCollection<TechnologyDto>> GetTechnologiesAsync(CityId cityId)
    {
        var technologies = await hohCoreDataRepository.GetTechnologiesAsync(cityId);
        return mapper.Map<IReadOnlyCollection<TechnologyDto>>(technologies);
    }
}
