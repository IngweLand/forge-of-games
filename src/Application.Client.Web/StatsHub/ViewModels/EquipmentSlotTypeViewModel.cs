using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;

public class EquipmentSlotTypeViewModel
{
    public required EquipmentSlotType SlotType { get; init; }
    public required string Name { get; init; }
    public required string IconUrl { get; init; }
}
