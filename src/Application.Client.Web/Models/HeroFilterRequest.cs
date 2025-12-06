using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Models;

public record HeroFilterRequest
{
    public static readonly HeroFilterRequest Empty = new();

    public string? AbilityTag { get; init; }
    public IReadOnlyCollection<HeroClassId> Classes { get; init; } = [];
    public IReadOnlyCollection<HeroStarClass> StarClasses { get; init; } = [];
    public IReadOnlyCollection<UnitColor> UnitColors { get; init; } = [];
    public IReadOnlyCollection<UnitType> UnitTypes { get; init; } = [];

    public bool IsEmpty()
    {
        if (this == Empty)
        {
            return true;
        }

        return Classes.Count == 0 && StarClasses.Count == 0 && UnitColors.Count == 0 && UnitTypes.Count == 0 &&
            AbilityTag == null;
    }
}
