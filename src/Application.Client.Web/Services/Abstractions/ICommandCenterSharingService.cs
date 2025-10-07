using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;
using Refit;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface ICommandCenterSharingService
{
    [Post("/commandCenter/sharedProfiles")]
    Task<ResourceCreatedResponse> ShareAsync([Body] BasicCommandCenterProfile profileDto);
    
    [Get("/commandCenter/sharedProfiles/{profileId}")]
    Task<BasicCommandCenterProfile?> GetSharedProfileAsync([AliasAs("profileId")] string profileId);
    
    [Post(FogUrlBuilder.ApiRoutes.COMMAND_CENTER_SHARED_SUBMISSION_ID)]
    Task<Guid> CreateSharedSubmissionIdAsync([Body] ShareSubmissionIdRequest request);
}
