using Ingweland.Fog.Dtos.Hoh;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface ICommonService
{
    [Get("/ages")]
    Task<IReadOnlyCollection<AgeDto>> GetAgesAsync();
}
