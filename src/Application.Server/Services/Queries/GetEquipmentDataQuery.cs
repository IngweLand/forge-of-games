using Ingweland.Fog.Application.Server.Factories.Interfaces;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Equipment;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetEquipmentDataQuery : IRequest<EquipmentDataDto>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromDays(365);
    public DateTimeOffset? Expiration { get; }
}

public class GetEquipmentDataQueryHandler(IEquipmentDataDtoFactory equipmentDataDtoFactory)
    : IRequestHandler<GetEquipmentDataQuery, EquipmentDataDto>
{
    public Task<EquipmentDataDto> Handle(GetEquipmentDataQuery request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(equipmentDataDtoFactory.Create());
    }
}
