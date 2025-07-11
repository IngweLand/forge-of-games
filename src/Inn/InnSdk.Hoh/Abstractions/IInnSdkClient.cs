using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

namespace Ingweland.Fog.InnSdk.Hoh.Abstractions;

public interface IInnSdkClient
{
    IBattleService BattleService { get; }
    ICityService CityService { get; }
    IPlayerService PlayerService { get; }
    IRankingsService RankingsService { get; }
    IStaticDataService StaticDataService { get; }
}
