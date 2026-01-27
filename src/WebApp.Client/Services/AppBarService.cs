using Ingweland.Fog.WebApp.Client.Models;

namespace Ingweland.Fog.WebApp.Client.Services;

public class AppBarService
{
    private readonly Dictionary<string, AppBarState> _states = new(StringComparer.OrdinalIgnoreCase);

    public event Action? OnChange;

    public AppBarState? GetState(string key)
    {
        _states.TryGetValue(key, out var state);

        return state;
    }

    public void SetState(string key, AppBarState state)
    {
        _states[key] = state;

        OnChange?.Invoke();
    }

    public void RemoveState(string key)
    {
        var state = GetState(key);
        if (state is null)
        {
            return;
        }

        _states.Remove(key);
        OnChange?.Invoke();
    }

    public void StateHasChanged(string key)
    {
        // if (_states.ContainsKey(key) && OnChange != null)
        // {
            OnChange?.Invoke();
        // }
    }
}
