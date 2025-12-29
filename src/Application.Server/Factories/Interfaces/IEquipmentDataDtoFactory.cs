using Ingweland.Fog.Dtos.Hoh.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;

namespace Ingweland.Fog.Application.Server.Factories.Interfaces;

public interface IEquipmentDataDtoFactory
{
    EquipmentDataDto Create(IReadOnlyCollection<EquipmentSetDefinition> setDefinitions);
}
