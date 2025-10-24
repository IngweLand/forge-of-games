using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Units;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IRelicCoreDataService
{
    [Get(FogUrlBuilder.ApiRoutes.RELICS_DATA)]
    Task<IReadOnlyCollection<RelicDto>> GetRelicsAsync();
}
