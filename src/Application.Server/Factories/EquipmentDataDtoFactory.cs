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
        var sets = Enum.GetValues<EquipmentSet>()
            .Where(x => x != EquipmentSet.Undefined)
            .ToDictionary(x => x.ToString(), localizationService.GetEquipmentSetName);
        sets = sets.Concat(Enum.GetValues<EquipmentSet>()
            .Where(x => x != EquipmentSet.Undefined)
            .SelectMany(x =>
                Enum.GetValues<EquipmentSlotType>().Where(y => y != EquipmentSlotType.Undefined)
                    .Select(y => (x, y)))
            .ToDictionary(x => $"{x.x}_{x.y}", x => localizationService.GetConcreteEquipmentSetName(x.x, x.y)))
            .ToDictionary();
        return new EquipmentDataDto
        {
            SlotTypes = SlotTypes.ToDictionary(x => x, localizationService.GetEquipmentSlotTypeName),
            StatAttributes = Enum.GetValues<StatAttribute>().ToList()
                .ToDictionary(x => x, localizationService.GetStatAttributeName),
            Sets = sets,
        };
    }
}
