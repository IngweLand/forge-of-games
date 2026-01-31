using Microsoft.Extensions.Logging;
using Refit;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public abstract class UiServiceBase(ILogger logger)
{
    protected ILogger Logger { get; } = logger;

    protected async Task<T> ExecuteSafeAsync<T>(Func<Task<T>> action, T fallback)
    {
        try
        {
            return await action();
        }
        catch (OperationCanceledException _)
        {
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected error in {Service}", GetType().FullName);
        }

        return fallback;
    }

    protected T ExecuteSafe<T>(Func<T> action, T fallback)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Unexpected error in {Service}", GetType().FullName);
        }

        return fallback;
    }
}
