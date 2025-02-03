using Ingweland.Fog.Application.Server.Settings;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Shared.Helpers;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class DefaultHohDataProvider : IHohDataProvider, IDisposable
{
    private readonly IDisposable? _optionsChangeToken;
    private readonly IProtobufSerializer _protobufSerializer;
    private readonly ILogger<DefaultHohDataProvider> _logger;
    private bool _disposed;

    private Task<Data>? _loadingTask;

    public DefaultHohDataProvider(IProtobufSerializer protobufSerializer,
        IOptionsMonitor<ResourceSettings> optionsMonitor,
        ILogger<DefaultHohDataProvider> logger)
    {
        _protobufSerializer = protobufSerializer;
        _logger = logger;

        _loadingTask = ReloadDataAsync(optionsMonitor.CurrentValue);

        _optionsChangeToken = optionsMonitor.OnChange((options, _) =>
        {
            if (!_disposed)
            {
                _loadingTask = ReloadDataAsync(options);
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

    public Task<Data> GetDataAsync()
    {
        return _loadingTask ?? Task.FromException<Data>(new InvalidOperationException("Data loading task not found."));
    }

    private async Task<Data> ReloadDataAsync(ResourceSettings options)
    {
        try
        {
            _logger.LogInformation("Starting data reload...");

            var data = await LoadAsync(options);
            _logger.LogInformation("Data reload completed successfully");
            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reloading data");
            throw;
        }
    }

    private async Task<Data> LoadAsync(ResourceSettings options)
    {
        var path = GetDataFilePath(options);
        var bytes = await File.ReadAllBytesAsync(path);
        return _protobufSerializer.DeserializeFromBytes<Data>(bytes);
    }

    private string GetDataFilePath(ResourceSettings options)
    {
        return $"{options.BaseUrl}/{options.HohDataPath}";
    }
}