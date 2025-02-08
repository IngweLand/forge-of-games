using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Refit;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface ICityPlannerSharingService
{
    [Post("/cityPlanner/sharedCities")]
    Task<ResourceCreatedResponse> ShareAsync([Body] HohCity city);
    
    [Get("/cityPlanner/sharedCities/{cityId}")]
    Task<HohCity?> GetSharedCityAsync([AliasAs("cityId")] string cityId);
}
