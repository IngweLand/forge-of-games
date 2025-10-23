using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Server.Factories;

public class EquipmentDataDtoFactory(IHohGameLocalizationService localizationService) : IEquipmentDataDtoFactory
{
    private static readonly List<EquipmentSlotType> SlotTypes =
    [
        EquipmentSlotType.Hand, EquipmentSlotType.Garment, EquipmentSlotType.Hat, EquipmentSlotType.Neck,
        EquipmentSlotType.Ring,
    ];

    public EquipmentDataDto Create()
    {
        return new EquipmentDataDto
        {
            SlotTypes = SlotTypes.ToDictionary(x => x, localizationService.GetEquipmentSlotTypeName),
            StatAttributes = Enum.GetValues<StatAttribute>().ToList()
                .ToDictionary(x => x, localizationService.GetStatAttributeName),
            Sets = Enum.GetValues<EquipmentSet>().ToList()
                .ToDictionary(x => x, localizationService.GetEquipmentSetName),
        };
    }
}
