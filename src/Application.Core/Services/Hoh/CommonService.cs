using AutoMapper;
using Ingweland.Fog.Application.Core.Interfaces;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Core.Services.Hoh;

public class CommonService(IHohCoreDataRepository hohCoreDataRepository, IMapper mapper) : ICommonService
{
    public async Task<IReadOnlyCollection<AgeDto>> GetAgesAsync()
    {
        var ages = await hohCoreDataRepository.GetAges();
        return mapper.Map<IReadOnlyCollection<AgeDto>>(ages);
    }
}
