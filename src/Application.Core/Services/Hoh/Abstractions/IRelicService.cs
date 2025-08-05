using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh.Units;
using Refit;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IRelicService
{
    [Get(FogUrlBuilder.ApiRoutes.RELICS)]
    Task<IReadOnlyCollection<RelicDto>> GetRelicsAsync();
}
