using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IRelicDtoFactory
{
     Task<RelicDto> CreateAsync(Relic relic);
}
