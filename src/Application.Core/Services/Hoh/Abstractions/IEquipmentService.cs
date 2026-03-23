using Ingweland.Fog.Dtos.Hoh.Equipment;

namespace Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

public interface IEquipmentService
{
    Task<EquipmentDataDto> GetEquipmentData();
}
