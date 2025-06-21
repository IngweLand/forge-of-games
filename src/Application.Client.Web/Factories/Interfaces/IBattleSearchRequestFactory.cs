using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBattleSearchRequestFactory
{
    BattleSearchRequest Create(string uri);
    Dictionary<string, object?> CreateQueryParams(BattleSearchRequest request);
}
