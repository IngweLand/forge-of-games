using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Refit;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface ISharedImageUploaderService
{
    [Post(FogUrlBuilder.ApiRoutes.UPLOAD_SHARED_IMAGE)]
    Task<ImageUploadResultDto> UploadAsync(ImageUploadDto data);
}
