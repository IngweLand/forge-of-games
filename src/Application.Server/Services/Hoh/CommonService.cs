using AutoMapper;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class CommonService(
    IHohCoreDataRepository hohCoreDataRepository,
    IMapper mapper,
    ILogger<TreasureHuntService> logger) : ICommonService
{
    public async Task<IReadOnlyCollection<AgeDto>> GetAgesAsync()
    {
        var ages = await hohCoreDataRepository.GetAges();
        return mapper.Map<IReadOnlyCollection<AgeDto>>(ages);
    }
}
