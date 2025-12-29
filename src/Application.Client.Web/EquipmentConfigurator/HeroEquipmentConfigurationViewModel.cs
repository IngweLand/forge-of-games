namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator;

public class HeroEquipmentConfigurationViewModel
{
    public HeroEquipmentItemViewModel? Garment { get; set; }
    public HeroEquipmentItemViewModel? Hand { get; set; }
    public HeroEquipmentItemViewModel? Hat { get; set; }
    public required string Id { get; init; }

    public bool IsInGame { get; init; }
    public HeroEquipmentItemViewModel? Neck { get; set; }
    public HeroEquipmentItemViewModel? Ring { get; set; }
}
