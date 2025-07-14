using Ingweland.Fog.Application.Core.Interfaces;
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
        const int maxRetries = 5;
        const int baseDelayMilliseconds = 500;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                _logger.LogInformation("Attempt {Attempt} to reload hoh core data...", attempt);
                var data = await LoadAsync(options);
                _logger.LogInformation("Hoh core data reload completed successfully on attempt {Attempt}", attempt);
                return data;
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                int delay = baseDelayMilliseconds * (int)Math.Pow(2, attempt - 1); // exponential backoff
                _logger.LogWarning(ex, "Attempt {Attempt} failed. Retrying in {Delay} ms...", attempt, delay);
                await Task.Delay(delay);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "All attempts to reload hoh core data failed.");
                throw;
            }
        }

        // Should never reach here due to throw in last catch block
        throw new InvalidOperationException("Failed to load hoh core data after all retry attempts.");
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