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
        const int maxRetries = 5;
        const int baseDelayMilliseconds = 500;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                Logger.LogInformation("Attempt {Attempt}: Starting data reload...", attempt);

                Load(options);

                Logger.LogInformation("Data reload completed successfully on attempt {Attempt}", attempt);
                return;
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                int delay = baseDelayMilliseconds * (int)Math.Pow(2, attempt - 1);
                Logger.LogWarning(ex, "Attempt {Attempt} failed. Retrying in {Delay} ms...", attempt, delay);
                Thread.Sleep(delay);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "All attempts to reload data failed.");
                throw;
            }
        }
    }

}
