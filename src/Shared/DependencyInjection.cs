using Ingweland.Fog.Shared.Helpers;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Ingweland.Fog.Shared;

public static class DependencyInjection
{
    public static void AddSharedServices(this IServiceCollection services)
    {

        services.AddSingleton<IProtobufSerializer, ProtobufSerializer>();
    }
}
