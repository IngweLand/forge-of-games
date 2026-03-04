namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityPlannerSettings
{
    private bool _diffModeIsActive = true;
    private bool _showEntityLevel = true;
    private bool _showEntityName = true;

    public bool DiffModeIsActive
    {
        get => _diffModeIsActive;
        set
        {
            if (_diffModeIsActive == value)
            {
                return;
            }

            _diffModeIsActive = value;
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

    public event Action? StateChanged;
}
