using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Relics;

namespace Ingweland.Fog.Application.Core.Factories.Interfaces;

public interface IRelicDtoFactory
{
    Task<RelicDto> CreateAsync(Relic relic);
}
