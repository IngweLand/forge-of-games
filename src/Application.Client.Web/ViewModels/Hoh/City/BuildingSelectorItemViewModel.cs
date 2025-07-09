using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;

public class BuildingSelectorItemViewModel
{
    private int _count;
    public required BuildingGroup BuildingGroup { get; init; }
    public required string Label { get; init; }

    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            if (_count < 0)
            {
                _count = 0;
            }
        }
    }
}
