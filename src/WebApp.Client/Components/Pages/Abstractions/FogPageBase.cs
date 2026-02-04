using Ingweland.Fog.Application.Client.Core.Localization;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;

[StreamRendering]
public abstract class FogPageBase : ComponentBase, IDisposable
{
    private readonly Dictionary<string, object> _dataToPersistedItems = new();
    private PersistingComponentStateSubscription _persistingSubscription;

    [Inject]
    protected PersistentComponentState ApplicationState { get; set; } = null!;

    [Inject]
    protected IJSInteropService JsInteropService { get; set; } = null!;

    [Inject]
    protected IStringLocalizer<FogResource> Loc { get; set; } = null!;

    protected virtual bool ShouldHidePageLoadingIndicator => true;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _persistingSubscription.Dispose();
        }
    }

    protected override Task OnInitializedAsync()
    {
        _persistingSubscription = ApplicationState.RegisterOnPersisting(PersistData);

        return Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && ShouldHidePageLoadingIndicator)
        {
            await JsInteropService.HideLoadingIndicatorAsync();
        }
    }

    protected async Task<T?> LoadWithPersistenceAsync<T>(string key, Func<Task<T?>> loadDataFunc)
    {
        var result = default(T);

        try
        {
            if (!ApplicationState.TryTakeFromJson<T?>(key, out var restored))
            {
                result = await loadDataFunc();
            }
            else
            {
                result = restored;
            }
        }
        catch (OperationCanceledException _)
        {
        }
        catch (ApiException apiEx) when (apiEx.InnerException is TaskCanceledException)
        {
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }

        if (result != null)
        {
            _dataToPersistedItems[key] = result;
        }
        else if (OperatingSystem.IsBrowser())
        {
            try
            {
                result = await loadDataFunc();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                throw;
            }
        }

        return result;
    }

    protected T? LoadWithPersistence<T>(string key, Func<T?> loadDataFunc)
    {
        var result = default(T);

        try
        {
            if (!ApplicationState.TryTakeFromJson<T?>(key, out var restored))
            {
                result = loadDataFunc();
            }
            else
            {
                result = restored;
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            throw;
        }

        if (result != null)
        {
            _dataToPersistedItems[key] = result;
        }
        else if (OperatingSystem.IsBrowser())
        {
            try
            {
                result = loadDataFunc();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                throw;
            }
        }

        return result;
    }

    protected virtual Task PersistData()
    {
        foreach (var item in _dataToPersistedItems)
        {
            ApplicationState.PersistAsJson(item.Key, item.Value);
        }

        return Task.CompletedTask;
    }
}
