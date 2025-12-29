using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces.Hoh;
using Ingweland.Fog.Dtos.Hoh.Equipment;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetEquipmentDataQuery : IRequest<EquipmentDataDto>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromDays(365);
    public DateTimeOffset? Expiration { get; }
}

public class GetEquipmentDataQueryHandler(IEquipmentDataDtoFactory equipmentDataDtoFactory, IHohCoreDataRepository hohCoreDataRepository)
    : IRequestHandler<GetEquipmentDataQuery, EquipmentDataDto>
{
    public async Task<EquipmentDataDto> Handle(GetEquipmentDataQuery request,
        CancellationToken cancellationToken)
    {
        var sets = await hohCoreDataRepository.GetEquipmentSetDefinitionsAsync();
        return equipmentDataDtoFactory.Create(sets);
    }
}
