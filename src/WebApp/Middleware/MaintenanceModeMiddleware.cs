using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Server.Settings;
using Microsoft.Extensions.Options;

namespace Ingweland.Fog.WebApp.Middleware;

public class MaintenanceModeMiddleware : IDisposable
{
    private readonly RequestDelegate _next;
    private readonly IDisposable? _optionsChangeToken;
    private bool _disposed;

    private MaintenanceModeSettings _settings;

    public MaintenanceModeMiddleware(RequestDelegate next, IOptionsMonitor<MaintenanceModeSettings> optionsMonitor)
    {
        _next = next;
        _settings = optionsMonitor.CurrentValue;
        _optionsChangeToken = optionsMonitor.OnChange((options, _) =>
        {
            if (!_disposed)
            {
                _settings = options;
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

    public async Task InvokeAsync(HttpContext context)
    {
        if (_settings.Enabled)
        {
            var allowedIPs = _settings.AllowedIPs;
            var clientIP = context.Connection.RemoteIpAddress?.ToString();

            if (clientIP != null && allowedIPs.Contains(clientIP))
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.StartsWithSegments(FogUrlBuilder.PageRoutes.MAINTENANCE_PAGE))
            {
                await _next(context);
                return;
            }

            context.Response.Redirect(FogUrlBuilder.PageRoutes.MAINTENANCE_PAGE);
            return;
        }

        await _next(context);
    }
}
