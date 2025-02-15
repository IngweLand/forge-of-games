using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

namespace Ingweland.Fog.InnSdk.Hoh.Abstractions;

public interface IInnSdkClient
{
    IStaticDataService StaticDataService { get; }
    IRankingsService RankingsService { get; }
}
