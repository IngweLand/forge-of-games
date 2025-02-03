namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityPlannerSettings
{
    private bool _showEntityName = true;
    private bool _showEntityLevel = true;

    public bool ShowEntityName
    {
        get => _showEntityName;
        set
        {
            if (_showEntityName == value)
            {
                return;
            }

            _showEntityName = value;
            StateChanged?.Invoke();
        }
    }

    public bool ShowEntityLevel
    {
        get => _showEntityLevel;
        set
        {
            if (_showEntityLevel == value)
            {
                return;
            }

            _showEntityLevel = value;
            StateChanged?.Invoke();
        }
    }

    public event Action? StateChanged;
}
