namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;

public class ResearchCalculatorTechnologyViewModel
{
    private ResearchCalculatorTechnologyState _state;
    public required string IconUrl { get; init; }
    public required string Id { get; init; }
    public required string Name { get; init; }

    public IReadOnlyCollection<ResearchCalculatorTechnologyViewModel> Parents { get; set; } =
        new List<ResearchCalculatorTechnologyViewModel>();

    public ResearchCalculatorTechnologyState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                OnStateChanged?.Invoke();
            }
        }
    }

    public event Action? OnStateChanged;
}
