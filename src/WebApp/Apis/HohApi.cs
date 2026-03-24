using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Server.CommandCenter.Commands;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.WebApp.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ingweland.Fog.WebApp.Apis;

public static class HohApi
{
    private static async Task<Results<Ok<InGameStartupData?>, InternalServerError<string>, NotFound>>
        GetInGameDataAsync(
            [AsParameters] HohServices services,
            HttpContext context, string inGameStartupDataId)
    {
        try
        {
            var inGameStartupData = await services.InGameStartupDataRepository.GetAsync(inGameStartupDataId);

            return inGameStartupData != null ? TypedResults.Ok(inGameStartupData)! : TypedResults.NotFound();
        }
        catch (Exception e)
        {
            return TypedResults.InternalServerError("Could not get saved profile.");
        }
    }

    private static async Task<Results<Ok<BasicCommandCenterProfile?>, BadRequest<string>, NotFound>>
        GetSharedProfileAsync(
            [AsParameters] HohServices services,
            HttpContext context, string profileId)
    {
        try
        {
            var profile = await services.CommandCenterProfileRepository.GetAsync(profileId);

            return profile != null ? TypedResults.Ok(profile)! : TypedResults.NotFound();
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest("Could not get shared profile.");
        }
    }

    private static async Task<Results<Ok<HohCity?>, BadRequest<string>, NotFound>>
        GetSharedCityAsync([AsParameters] HohServices services, HttpContext context, string cityId)
    {
        try
        {
            var city = await services.HohCityRepository.GetAsync(cityId);

            return city != null ? TypedResults.Ok(city)! : TypedResults.NotFound();
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest("Could not get shared city.");
        }
    }

    private static async
        Task<Results<Ok<ResourceCreatedResponse>, BadRequest<string>, InternalServerError<string>>>
        ImportInGameDataAsync(
            [AsParameters] HohServices services,
            HttpContext context, [FromBody] ImportInGameStartupDataRequestDto importRequestDto)
    {
        InGameStartupData inGameStartupData;
        try
        {
            inGameStartupData =
                await services.InGameStartupDataProcessingService.ParseStartupData(importRequestDto.InGameStartupData);
        }
        catch (Exception e)
        {
            services.Logger.LogError(e, "Could not process hoh startup data.");
            return TypedResults.BadRequest("Could not process startup data.");
        }

        string newId;
        try
        {
            newId = await services.InGameStartupDataRepository.SaveAsync(inGameStartupData);
        }
        catch (Exception e)
        {
            services.Logger.LogError(e, "Could not save hoh startup data.");
            return TypedResults.InternalServerError("Could not save startup data.");
        }

        var result = new ResourceCreatedResponse
        {
            ApiResourceUrl = $"https://{context.Request.Host}{context.Request.Path}{newId}",
            WebResourceUrl = $"https://{context.Request.Host}/inGameData/{newId}",
            ResourceId = newId,
        };
        return TypedResults.Ok(result);
    }

    public static RouteGroupBuilder MapHohApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/hoh");

        api.MapPost("/cityPlanner/sharedCities", ShareCityAsync);
        api.MapGet("/cityPlanner/sharedCities/{cityId}", GetSharedCityAsync);

        api.MapPost("/commandCenter/sharedProfiles", ShareProfileAsync);
        api.MapGet("/commandCenter/sharedProfiles/{profileId}", GetSharedProfileAsync);
        api.MapPost(FogUrlBuilder.ApiRoutes.COMMAND_CENTER_SHARED_SUBMISSION_ID, ShareSubmissionIdAsync);

        api.MapPost("/inGameData/", ImportInGameDataAsync).RequireCors(PolicyNames.CORS_IN_GAME_DATA_IMPORT_POLICY);
        api.MapGet("/inGameData/{inGameStartupDataId}", GetInGameDataAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.WIKI_EXTRACT, GetWikiExtractAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.IN_GAME_EVENTS_TEMPLATE, GetInGameEventsAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.CURRENT_IN_GAME_EVENT_TEMPLATE, GetCurrentInGameEventAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.ANNUAL_BUDGET_TEMPLATE, GetAnnualBudgetAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.HOH_CORE_DATA, GetHohCoreDataAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.HOH_LOCALIZATION_DATA, GetHohLocalizationDataAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.HOH_CORE_DATE_VERSION, GetHohCoreDataVersionAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.HERO_ABILITY_FEATURES, GetHeroAbilityFeaturesAsync);

        return api;
    }

    private static async Task<Results<Ok<AnnualBudgetDto>, NotFound, BadRequest<string>>>
        GetAnnualBudgetAsync([AsParameters] StatsServices services, HttpContext context,
            [AsParameters] GetAnnualBudgetQuery query, CancellationToken ct = default)
    {
        var result = await services.Mediator.Send(query, ct);

        if (result != null)
        {
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound();
    }

    private static async Task<Ok<IReadOnlyCollection<InGameEventDto>>>
        GetInGameEventsAsync([AsParameters] StatsServices services, HttpContext context, string worldId,
            EventDefinitionId eventDefinitionId, CancellationToken ct)
    {
        var query = new GetEventsQuery
        {
            WorldId = worldId,
            EventDefinitionId = eventDefinitionId,
        };
        var result = await services.Mediator.Send(query, ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<InGameEventDto>, NotFound, BadRequest<string>>>
        GetCurrentInGameEventAsync([AsParameters] StatsServices services, HttpContext context, string worldId,
            EventDefinitionId eventDefinitionId, CancellationToken ct)
    {
        var query = new GetCurrentInGameEventQuery(worldId, eventDefinitionId);
        var result = await services.Mediator.Send(query, ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Results<Ok<WikipediaResponseDto>, NotFound, BadRequest<string>>>
        GetWikiExtractAsync([AsParameters] HohServices services, HttpContext context,
            [FromQuery] string title, [FromQuery] string language)
    {
        var result = await services.WikipediaService.GetArticleAbstractAsync(title, language);
        if (result != null)
        {
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound();
    }

    private static async Task<Results<Ok<ResourceCreatedResponse>, BadRequest<string>>> ShareProfileAsync(
        [AsParameters] HohServices services,
        HttpContext context, [FromBody] BasicCommandCenterProfile profileDto)
    {
        try
        {
            var newId = await services.CommandCenterProfileRepository.SaveAsync(profileDto);
            var result = new ResourceCreatedResponse
            {
                ApiResourceUrl = $"https://{context.Request.Host}{context.Request.Path}/{newId}",
                WebResourceUrl = $"https://{context.Request.Host}/commandCenter/sharedProfiles/{newId}",
                ResourceId = newId,
            };
            return TypedResults.Ok(result);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest("Could not create the share.");
        }
    }

    private static async Task<Results<Ok<ResourceCreatedResponse>, BadRequest<string>>> ShareCityAsync(
        [AsParameters] HohServices services,
        HttpContext context, [FromBody] HohCity city)
    {
        try
        {
            var newId = await services.HohCityRepository.SaveAsync(city);
            var result = new ResourceCreatedResponse
            {
                ApiResourceUrl = $"https://{context.Request.Host}{context.Request.Path}/{newId}",
                WebResourceUrl = $"https://{context.Request.Host}/cityPlanner/sharedCities/{newId}",
                ResourceId = newId,
            };
            return TypedResults.Ok(result);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest("Could not create the share.");
        }
    }

    private static async Task<Results<Ok<Guid>, BadRequest<string>>> ShareSubmissionIdAsync(
        [AsParameters] HohServices services, [FromBody] ShareSubmissionIdRequest request)
    {
        try
        {
            var cmd = new CreateSharedSubmissionIdCommand(request.SubmissionId);
            var newId = await services.Mediator.Send(cmd);
            return TypedResults.Ok(newId);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest("Could not create shared submission id.");
        }
    }

    private static async Task<Ok<VersionedResponse<byte[]?>>> GetHohCoreDataAsync([AsParameters] HohServices services,
        HttpContext context, [AsParameters] GetHohCoreDataQuery query)
    {
        var result = await services.Mediator.Send(query);

        return TypedResults.Ok(result);
    }

    private static async Task<Ok<VersionDto>> GetHohCoreDataVersionAsync([AsParameters] HohServices services,
        HttpContext context, [AsParameters] GetHohCoreDataVersionQuery query)
    {
        var result = await services.Mediator.Send(query);

        return TypedResults.Ok(result);
    }

    private static async Task<Ok<VersionedResponse<byte[]?>>> GetHohLocalizationDataAsync(
        [AsParameters] HohServices services,
        HttpContext context, [AsParameters] GetHohLocalizationDataQuery query)
    {
        var result = await services.Mediator.Send(query);

        return TypedResults.Ok(result);
    }

    private static async Task<Ok<IReadOnlyCollection<HeroAbilityFeaturesDto>>> GetHeroAbilityFeaturesAsync(
        [AsParameters] HohServices services, HttpContext context)
    {
        var result = await services.Mediator.Send(new GetHeroAbilityFeaturesQuery());

        return TypedResults.Ok(result);
    }
}
