using FluentResults;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services.Commands;

public record UploadSharedImageCommand(ImageUploadDto Request) : IRequest<Result<ImageUploadResultDto>>;

public class UploadSharedImageHandler(ISharedImageStorageRepository storageRepository)
    : IRequestHandler<UploadSharedImageCommand, Result<ImageUploadResultDto>>
{
    public async Task<Result<ImageUploadResultDto>> Handle(UploadSharedImageCommand command,
        CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid().ToString("N");
        var result = await storageRepository.SaveAsync(id, command.Request.ContentType, command.Request.Data);
        return result.IsSuccess ? Result.Ok(new ImageUploadResultDto {Url = result.Value}) : result.ToResult();
    }
}
