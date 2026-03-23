using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IRelicCoreDataService
{
    Task<IReadOnlyCollection<RelicDto>> GetRelicsAsync();
}
