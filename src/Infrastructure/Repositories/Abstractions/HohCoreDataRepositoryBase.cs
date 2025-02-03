using Ingweland.Fog.Application.Server.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public abstract class HohCoreDataRepositoryBase<TConcreteRepository> : IDisposable
{
    protected ILogger Logger { get; }
    private readonly IDisposable? _optionsChangeToken;
    private bool _disposed;

    protected HohCoreDataRepositoryBase(IOptionsMonitor<ResourceSettings> optionsMonitor,
        ILogger<TConcreteRepository> logger)
    {
        Logger = logger;

        ReloadData(optionsMonitor.CurrentValue);

        _optionsChangeToken = optionsMonitor.OnChange((options, _) =>
        {
            if (!_disposed)
            {
                ReloadData(options);
            }
        });
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _optionsChangeToken?.Dispose();
    }

    protected abstract void Load(ResourceSettings options);
    private void ReloadData(ResourceSettings options)
    {
        try
        {
            Logger.LogInformation("Starting data reload...");

            Load(options);

            Logger.LogInformation("Data reload completed successfully");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error reloading data");
        }
    }
}
