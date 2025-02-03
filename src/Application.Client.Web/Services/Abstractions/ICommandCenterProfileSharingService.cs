using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.CommandCenter;
using Ingweland.Fog.Models.Fog.Entities;
using Refit;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface ICommandCenterProfileSharingService
{
    [Post("/commandCenter/sharedProfiles")]
    Task<ResourceCreatedResponse> ShareAsync([Body] BasicCommandCenterProfile profileDto);
    
    [Get("/commandCenter/sharedProfiles/{profileId}")]
    Task<BasicCommandCenterProfile?> GetSharedProfileAsync([AliasAs("profileId")] string profileId);
}
