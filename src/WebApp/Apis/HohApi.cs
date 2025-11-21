using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Application.Server.CommandCenter.Commands;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.Equipment;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Ingweland.Fog.WebApp.Constants;
using Ingweland.Fog.WebApp.Extensions;
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

        api.MapProtobufGet("/heroes/basic", GetHeroesBasicDataAsync);
        api.MapProtobufGet("/heroes/{heroId}", GetHeroAsync);

        api.MapProtobufGet("/city/barracks", GetAllBarracksAsync);
        api.MapProtobufGet("/city/barracks/{unitType}", GetBarracksAsync);
        api.MapProtobufGet("/city/buildingCategories/{cityId}", GetBuildingCategoriesAsync);
        api.MapProtobufGet("/city/buildingGroup/{cityId}/{group}", GetBuildingGroupAsync);
        api.MapProtobufGet("/city/{cityId}/expansions", GetExpansionsAsync);
        api.MapProtobufGet("/city/{cityId}/buildings", GetBuildingsAsync);

        api.MapProtobufGet("/city/wonders/basic", GetWonderBasicDataAsync);
        api.MapProtobufGet("/city/wonders/{id}", GetWonderAsync);

        api.MapProtobufGet("/technologies/{cityId}", GetTechnologiesAsync);

        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.CAMPAIGN_CONTINENTS_BASIC_DATA_PATH,
            GetCampaignContinentsBasicDataAsync);
        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.CAMPAIGN_REGION_TEMPLATE, GetCampaignRegionAsync);
        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.CAMPAIGN_REGION_BASIC_DATA_TEMPLATE,
            GetCampaignRegionBasicDataAsync);

        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.TREASURE_HUNT_DIFFICULTIES_PATH, GetTreasureHuntDifficultiesAsync);
        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.TREASURE_HUNT_STAGE_TEMPLATE, GetTreasureHuntStageAsync);
        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.TREASURE_HUNT_ENCOUNTERS_BASIC_DATA_PATH,
            GetTreasureHuntEncountersBasicDataAsync);

        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.COMMON_AGES, GetAgesAsync);
        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.COMMON_RESOURCES, GetResourcesAsync);
        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.COMMON_CITIES, GetCitiesAsync);
        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.COMMON_PVP_TIERS, GetPvpTiersAsync);

        api.MapProtobufGet(FogUrlBuilder.ApiRoutes.RELICS_DATA, GetRelicsDataAsync);

        api.MapProtobufGet("/cityPlanner/data/{cityId}", GetCityPlannerDataAsync);
        api.MapPost("/cityPlanner/sharedCities", ShareCityAsync);
        api.MapGet("/cityPlanner/sharedCities/{cityId}", GetSharedCityAsync);

        api.MapProtobufGet("/commandCenter/data", GetCommandCenterDataAsync);
        api.MapPost("/commandCenter/sharedProfiles", ShareProfileAsync);
        api.MapGet("/commandCenter/sharedProfiles/{profileId}", GetSharedProfileAsync);
        api.MapPost(FogUrlBuilder.ApiRoutes.COMMAND_CENTER_SHARED_SUBMISSION_ID, ShareSubmissionIdAsync);

        api.MapPost("/inGameData/", ImportInGameDataAsync).RequireCors(PolicyNames.CORS_IN_GAME_DATA_IMPORT_POLICY);
        api.MapGet("/inGameData/{inGameStartupDataId}", GetInGameDataAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.WIKI_EXTRACT, GetWikiExtractAsync);
        api.MapGet(FogUrlBuilder.ApiRoutes.EQUIPMENT_DATA, GetEquipmentDataAsync);

        api.MapGet(FogUrlBuilder.ApiRoutes.IN_GAME_EVENTS_TEMPLATE, GetInGameEventsAsync);

        return api;
    }

    private static async Task<Ok<EquipmentDataDto>>
        GetEquipmentDataAsync([AsParameters] StatsServices services, HttpContext context, CancellationToken ct)
    {
        var result = await services.EquipmentService.GetEquipmentData(ct);
        return TypedResults.Ok(result);
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

    private static async Task GetAgesAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var ages = await services.CommonService.GetAgesAsync();
        await WriteToResponseAsync(context, ages, services.ProtobufSerializer);
    }

    private static async Task GetResourcesAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var resources = await services.CommonService.GetResourcesAsync();
        await WriteToResponseAsync(context, resources, services.ProtobufSerializer);
    }

    private static async Task GetCitiesAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var cities = await services.CityService.GetCitiesAsync();
        await WriteToResponseAsync(context, cities, services.ProtobufSerializer);
    }

    private static async Task GetPvpTiersAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var cities = await services.CommonService.GetPvpTiersAsync();
        await WriteToResponseAsync(context, cities, services.ProtobufSerializer);
    }

    private static async Task GetRelicsDataAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var relics = await services.RelicCoreDataService.GetRelicsAsync();
        await WriteToResponseAsync(context, relics, services.ProtobufSerializer);
    }

    private static async Task GetBarracksAsync([AsParameters] HohServices services,
        HttpContext context, UnitType unitType)
    {
        var barracks = await services.CityService.GetBarracks(unitType);

        await WriteToResponseAsync(context, barracks, services.ProtobufSerializer);
    }

    private static async Task GetAllBarracksAsync([AsParameters] HohServices services, HttpContext context)
    {
        var barracks = await services.CityService.GetAllBarracks();

        await WriteToResponseAsync(context, barracks, services.ProtobufSerializer);
    }

    private static async Task GetBuildingCategoriesAsync([AsParameters] HohServices services,
        HttpContext context, CityId cityId)
    {
        var types = await services.CityService.GetBuildingCategoriesAsync(cityId);

        await WriteToResponseAsync(context, types, services.ProtobufSerializer);
    }

    private static async Task GetBuildingGroupAsync([AsParameters] HohServices services,
        HttpContext context, CityId cityId, BuildingGroup group)
    {
        var buildingGroup = await services.CityService.GetBuildingGroupAsync(cityId, group);
        if (buildingGroup == null)
        {
            services.Logger.LogError($"{nameof(GetBuildingGroupAsync)} - Could not find building group. CityId: {cityId
            }, group: {group}");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await WriteToResponseAsync(context, buildingGroup, services.ProtobufSerializer);
    }

    private static async Task GetBuildingsAsync([AsParameters] HohServices services,
        HttpContext context, CityId cityId)
    {
        var types = await services.CityService.GetBuildingsAsync(cityId);

        await WriteToResponseAsync(context, types, services.ProtobufSerializer);
    }

    private static async Task GetCityPlannerDataAsync([AsParameters] HohServices services,
        HttpContext context, CityId cityId)
    {
        var cpd = await services.CityService.GetCityPlannerDataAsync(cityId);

        await WriteToResponseAsync(context, cpd, services.ProtobufSerializer);
    }

    private static async Task GetCommandCenterDataAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var cpd = await services.CommandCenterService.GetCommandCenterDataAsync();

        await WriteToResponseAsync(context, cpd, services.ProtobufSerializer);
    }

    private static async Task GetExpansionsAsync([AsParameters] HohServices services,
        HttpContext context, CityId cityId)
    {
        var types = await services.CityService.GetExpansionsAsync(cityId);

        await WriteToResponseAsync(context, types, services.ProtobufSerializer);
    }

    private static async Task GetHeroAsync([AsParameters] HohServices services,
        HttpContext context, string heroId)
    {
        var hero = await services.UnitService.GetHeroAsync(heroId);
        if (hero == null)
        {
            services.Logger.LogError($"{nameof(GetHeroAsync)} - Could not find hero with id {heroId}");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await WriteToResponseAsync(context, hero, services.ProtobufSerializer);
    }

    private static async Task GetHeroesBasicDataAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var heroes = await services.UnitService.GetHeroesBasicDataAsync();
        await WriteToResponseAsync(context, heroes, services.ProtobufSerializer);
    }

    private static async Task GetTechnologiesAsync([AsParameters] HohServices services,
        HttpContext context, CityId cityId)
    {
        var types = await services.ResearchService.GetTechnologiesAsync(cityId);

        await WriteToResponseAsync(context, types, services.ProtobufSerializer);
    }

    private static async Task GetCampaignRegionAsync([AsParameters] HohServices services,
        HttpContext context, RegionId regionId)
    {
        var region = await services.CampaignService.GetRegionAsync(regionId);
        if (region != null)
        {
            await WriteToResponseAsync(context, region, services.ProtobufSerializer);
        }
        else
        {
            WriteNotFoundToResponseAsync(context);
        }
    }

    private static async Task GetCampaignRegionBasicDataAsync([AsParameters] HohServices services,
        HttpContext context, RegionId regionId)
    {
        var region = await services.CampaignService.GetRegionBasicDataAsync(regionId);
        await WriteToResponseAsync(context, region, services.ProtobufSerializer);
    }

    private static async Task GetTreasureHuntStageAsync([AsParameters] HohServices services,
        HttpContext context, int difficulty, int stageIndex)
    {
        var stage = await services.TreasureHuntService.GetStageAsync(difficulty, stageIndex);
        if (stage != null)
        {
            await WriteToResponseAsync(context, stage, services.ProtobufSerializer);
        }
        else
        {
            WriteNotFoundToResponseAsync(context);
        }
    }

    private static async Task GetTreasureHuntEncountersBasicDataAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var map = await services.TreasureHuntService.GetTreasureHuntEncountersBasicDataAsync();
        await WriteToResponseAsync(context, map, services.ProtobufSerializer);
    }

    private static async Task GetCampaignContinentsBasicDataAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var data = await services.CampaignService.GetCampaignContinentsBasicDataAsync();

        await WriteToResponseAsync(context, data, services.ProtobufSerializer);
    }

    private static async Task GetTreasureHuntDifficultiesAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var difficulties = await services.TreasureHuntService.GetDifficultiesAsync();

        await WriteToResponseAsync(context, difficulties, services.ProtobufSerializer);
    }

    private static async Task GetWonderAsync([AsParameters] HohServices services,
        HttpContext context, WonderId id)
    {
        var wonder = await services.CityService.GetWonderAsync(id);
        if (wonder == null)
        {
            services.Logger.LogError($"{nameof(GetWonderAsync)} - Could not find wonder with id {id}");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await WriteToResponseAsync(context, wonder, services.ProtobufSerializer);
    }

    private static async Task GetWonderBasicDataAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var heroes = await services.CityService.GetWonderBasicDataAsync();
        await WriteToResponseAsync(context, heroes, services.ProtobufSerializer);
    }

    private static ValueTask WriteToResponseAsync(HttpContext context, object payload, IProtobufSerializer serializer)
    {
        var bytes = serializer.SerializeToBytes(payload);
        context.Response.ContentType = "application/x-protobuf";
        context.Response.StatusCode = StatusCodes.Status200OK;
        return context.Response.Body.WriteAsync(bytes);
    }

    private static void WriteNotFoundToResponseAsync(HttpContext context)
    {
        context.Response.ContentType = "application/x-protobuf";
        context.Response.StatusCode = StatusCodes.Status404NotFound;
    }
}
