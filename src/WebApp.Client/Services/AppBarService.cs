using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Services;

public class AppBarService
{
    public RenderFragment? Content { get; private set; }

    public event Action? OnChange;

    public void Set(RenderFragment? content)
    {
        Content = content;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        Set(null);
    }

    public void StateHasChanged()
    {
        OnChange?.Invoke();
    }
}
