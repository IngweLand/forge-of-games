using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IBattleDefinitionIdFactory
{
    Task<string> Create(BattleSearchRequest request);
}
