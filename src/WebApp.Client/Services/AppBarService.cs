using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Services;

public class AppBarService
{
    private RenderFragment? _content;
    private bool _shouldHideTitle;

    public RenderFragment? Content
    {
        get => _content;
        set
        {
            if (_content == value)
            {
                return;
            }

            _content = value;
            OnChange?.Invoke();
        }
    }

    public bool ShouldHideTitle
    {
        get => _shouldHideTitle;
        set
        {
            if (_shouldHideTitle == value)
            {
                return;
            }

            _shouldHideTitle = value;
            OnChange?.Invoke();
        }
    }

    public event Action? OnChange;

    public void Reset()
    {
        Content = null;
        ShouldHideTitle = false;
    }

    public void StateHasChanged()
    {
        OnChange?.Invoke();
    }
}
