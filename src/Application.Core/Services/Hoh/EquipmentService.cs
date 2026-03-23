using Ingweland.Fog.Application.Core.Factories.Interfaces;
using Ingweland.Fog.Application.Core.Repository.Abstractions;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Equipment;

namespace Ingweland.Fog.Application.Core.Services.Hoh;

public class EquipmentService(
    IEquipmentDataDtoFactory equipmentDataDtoFactory,
    IHohCoreDataRepository hohCoreDataRepository) : IEquipmentService
{
    public async Task<EquipmentDataDto> GetEquipmentData()
    {
        var sets = await hohCoreDataRepository.GetEquipmentSetDefinitionsAsync();
        return equipmentDataDtoFactory.Create(sets);
    }
}
