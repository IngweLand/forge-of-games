using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public record EquipmentFilterRequest
{
    public static readonly EquipmentFilterRequest Empty = new();
    public bool HideEquipped { get; init; }
    public IReadOnlyCollection<StatAttribute> MainAttributes { get; init; } = [];
    public bool OnlyUnlockedSubAttributes { get; init; }
    public IReadOnlyCollection<EquipmentRarity> Rarities { get; init; } = [];

    public IReadOnlyCollection<EquipmentSet> Sets { get; init; } = [];
    public IReadOnlyCollection<StatAttribute> SubAttributes { get; init; } = [];

    public bool IsEmpty()
    {
        if (this == Empty)
        {
            return true;
        }

        return Sets.Count == 0 && Rarities.Count == 0 && MainAttributes.Count == 0 && SubAttributes.Count == 0 &&
            !OnlyUnlockedSubAttributes && !HideEquipped;
    }
}
