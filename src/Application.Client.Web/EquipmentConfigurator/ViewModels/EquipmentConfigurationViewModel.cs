using CommunityToolkit.Mvvm.ComponentModel;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.ViewModels;

public class EquipmentConfigurationViewModel : ObservableObject
{
    private EquipmentItemViewModel2? _garment;

    private EquipmentItemViewModel2? _hand;

    private EquipmentItemViewModel2? _hat;

    private EquipmentItemViewModel2? _neck;

    private EquipmentItemViewModel2? _ring;

    private IReadOnlyCollection<IconLabelsItemViewModel> _stats = [];

    public EquipmentItemViewModel2? Garment
    {
        get => _garment;
        set => SetProperty(ref _garment, value);
    }

    public EquipmentItemViewModel2? Hand
    {
        get => _hand;
        set => SetProperty(ref _hand, value);
    }

    public EquipmentItemViewModel2? Hat
    {
        get => _hat;
        set => SetProperty(ref _hat, value);
    }

    public required HeroBasicViewModel Hero { get; init; }
    public required string Id { get; init; }

    public bool IsInGame { get; init; }

    public EquipmentItemViewModel2? Neck
    {
        get => _neck;
        set => SetProperty(ref _neck, value);
    }

    public EquipmentItemViewModel2? Ring
    {
        get => _ring;
        set => SetProperty(ref _ring, value);
    }

    public IReadOnlyCollection<IconLabelsItemViewModel> Stats
    {
        get => _stats;
        set => SetProperty(ref _stats, value);
    }

    public EquipmentItemViewModel2? Get(EquipmentSlotType slot)
    {
        return slot switch
        {
            EquipmentSlotType.Garment => Garment,
            EquipmentSlotType.Hand => Hand,
            EquipmentSlotType.Hat => Hat,
            EquipmentSlotType.Neck => Neck,
            EquipmentSlotType.Ring => Ring,
            _ => null,
        };
    }
}
