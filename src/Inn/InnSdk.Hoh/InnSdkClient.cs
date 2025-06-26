using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

namespace Ingweland.Fog.InnSdk.Hoh;

public class InnSdkClient(
    Lazy<IStaticDataService> staticDataService,
    Lazy<IRankingsService> rankingsService,
    Lazy<IBattleService> battleService)
    : IInnSdkClient
{
    public IBattleService BattleService => battleService.Value;
    public IRankingsService RankingsService => rankingsService.Value;
    public IStaticDataService StaticDataService => staticDataService.Value;
}
