using Microsoft.Extensions.DependencyInjection;

namespace Ingweland.Fog.HohCoreDataParserSdk;

public static class DependencyInjection
{
    public static IServiceCollection AddHohCoreDataParserSdkServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddTransient<LocalizationParser>();
        services.AddTransient<GameDesignDataParser>();

        return services;
    }
}
