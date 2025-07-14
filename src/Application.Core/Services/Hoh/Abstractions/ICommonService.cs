using Ingweland.Fog.Dtos.Hoh;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICommonService
{
    Task<IReadOnlyCollection<AgeDto>> GetAgesAsync();
}
