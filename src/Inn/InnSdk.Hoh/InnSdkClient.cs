using Ingweland.Fog.InnSdk.Hoh.Abstractions;
using Ingweland.Fog.InnSdk.Hoh.Services.Abstractions;

namespace Ingweland.Fog.InnSdk.Hoh;

public class InnSdkClient(Lazy<IStaticDataService> staticDataService) : IInnSdkClient
{
    public IStaticDataService StaticDataService => staticDataService.Value;
}
