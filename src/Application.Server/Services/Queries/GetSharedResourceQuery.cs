using FluentResults;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using MediatR;

public record GetSharedResourceQuery(string ShareId) : IRequest<Result<SharedDataDto>>;

public class GetSharedResourceHandler(IFogSharedDataStorageRepository storageRepository)
    : IRequestHandler<GetSharedResourceQuery, Result<SharedDataDto>>
{
    public async Task<Result<SharedDataDto>> Handle(GetSharedResourceQuery request, CancellationToken ct)
    {
        var result = await storageRepository.GetAsStringAsync(request.ShareId);
        if (result.IsSuccess)
        {
            return Result.Ok(new SharedDataDto
            {
                Data = result.Value,
            });
        }

        return result.ToResult();
    }
}
