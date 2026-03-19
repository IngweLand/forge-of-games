using Ingweland.Fog.Application.Server.Services.Interfaces;
using Ingweland.Fog.Application.Server.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public abstract class ReloadableDataProviderBase<TData> : IDisposable
{
    private readonly IHohDataCacheClearingService _cacheClearingService;
    private readonly IDisposable? _optionsChangeToken;
    private bool _disposed;

    private Task<TData> _loadingTask;

    protected ReloadableDataProviderBase(IOptionsMonitor<ResourceSettings> optionsMonitor,
        IHohDataCacheClearingService cacheClearingService,
        ILogger<ReloadableDataProviderBase<TData>> logger)
    {
        _cacheClearingService = cacheClearingService;
        Logger = logger;

        _loadingTask = ReloadDataAsync(optionsMonitor.CurrentValue);

        _optionsChangeToken = optionsMonitor.OnChange((options, _) =>
        {
            if (!_disposed)
            {
                _loadingTask = ReloadDataAsync(options);
                Version = Guid.NewGuid();
            }
        });
    }

    protected ILogger<ReloadableDataProviderBase<TData>> Logger { get; }
    public Guid Version { get; private set; } = Guid.NewGuid();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task<TData> GetDataAsync()
    {
        return _loadingTask;
    }

    public TData GetData()
    {
        return _loadingTask.GetAwaiter().GetResult();
    }

    private async Task<TData> ReloadDataAsync(ResourceSettings options)
    {
        try
        {
            Logger.LogInformation("Reloading Azure Blob Storage data...");
            _cacheClearingService.Clear();
            var data = await LoadAsync(options);
            Logger.LogInformation("Azure Blob Storage data reloaded successfully.");
            return data;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to reload Azure Blob Storage data.");
            throw;
        }
    }

    protected abstract Task<TData> LoadAsync(ResourceSettings options);

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _disposed = true;
            _optionsChangeToken?.Dispose();
            _loadingTask.Dispose();
        }
    }
}
