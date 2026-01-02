namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;

public class EquipmentConfigurationViewModel
{
    public EquipmentItemViewModel2? Garment { get; set; }
    public EquipmentItemViewModel2? Hand { get; set; }
    public EquipmentItemViewModel2? Hat { get; set; }
    public required string Id { get; init; }

    public bool IsInGame { get; init; }
    public EquipmentItemViewModel2? Neck { get; set; }
    public EquipmentItemViewModel2? Ring { get; set; }
}
