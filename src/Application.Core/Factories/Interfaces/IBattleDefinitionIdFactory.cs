using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Core.Factories.Interfaces;

public interface IBattleDefinitionIdFactory
{
    string Create(BattleSearchRequest request);
}
