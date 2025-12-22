using FluentResults;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Commands;

public record CreateShareCommand(SharedDataDto Content) : IRequest<Result<CreatedShareDto>>;

public class CreateShareHandler(IFogSharedDataStorageRepository storageRepository)
    : IRequestHandler<CreateShareCommand, Result<CreatedShareDto>>
{
    public async Task<Result<CreatedShareDto>> Handle(CreateShareCommand request, CancellationToken ct)
    {
        var id = Guid.NewGuid().ToString("N");
        var result = await storageRepository.SaveAsync(id, request.Content.Data);
        return result.IsSuccess ? Result.Ok(new CreatedShareDto {Id = id}) : result;
    }
}
