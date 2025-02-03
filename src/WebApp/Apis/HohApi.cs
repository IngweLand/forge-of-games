using Ingweland.Fog.Dtos.Hoh;
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
    public static async Task<Results<Ok<InGameStartupData?>, InternalServerError<string>, NotFound>>
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

    public static async Task<Results<Ok<BasicCommandCenterProfile?>, BadRequest<string>, NotFound>>
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

    public static async
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
        api.MapProtobufGet("/heroes/{heroId}/ability", GetHeroAbilityAsync);

        api.MapProtobufGet("/city/barracks/{unitType}", GetBarracksAsync);
        api.MapProtobufGet("/city/buildingCategories/{cityId}", GetBuildingCategoriesAsync);
        api.MapProtobufGet("/city/buildingGroup/{cityId}/{group}", GetBuildingGroupAsync);
        api.MapProtobufGet("/city/{cityId}/expansions", GetExpansionsAsync);
        api.MapProtobufGet("/city/{cityId}/buildings", GetBuildingsAsync);

        api.MapProtobufGet("/city/wonders/basic", GetWonderBasicDataAsync);
        api.MapProtobufGet("/city/wonders/{id}", GetWonderAsync);

        api.MapProtobufGet("/technologies/{cityId}", GetTechnologiesAsync);

        api.MapProtobufGet("/ages", GetAgesAsync);

        api.MapProtobufGet("/cityPlanner/data/{cityId}", GetCityPlannerDataAsync);

        api.MapProtobufGet("/commandCenter/data", GetCommandCenterDataAsync);
        api.MapPost("/commandCenter/sharedProfiles", ShareProfileAsync);
        api.MapGet("/commandCenter/sharedProfiles/{profileId}", GetSharedProfileAsync);

        api.MapPost("/inGameData/", ImportInGameDataAsync).RequireCors(PolicyNames.CORS_IN_GAME_DATA_IMPORT_POLICY);
        api.MapGet("/inGameData/{inGameStartupDataId}", GetInGameDataAsync);

        return api;
    }

    public static async Task<Results<Ok<ResourceCreatedResponse>, BadRequest<string>>> ShareProfileAsync(
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

    private static async Task GetAgesAsync([AsParameters] HohServices services,
        HttpContext context)
    {
        var ages = await services.CommonService.GetAgesAsync();
        await WriteToResponseAsync(context, ages, services.ProtobufSerializer);
    }

    private static async Task GetBarracksAsync([AsParameters] HohServices services,
        HttpContext context, UnitType unitType)
    {
        var barracks = await services.CityService.GetBarracks(unitType);

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

    private static async Task GetHeroAbilityAsync([AsParameters] HohServices services,
        HttpContext context, string heroId)
    {
        var ability = await services.UnitService.GetHeroAbilityAsync(heroId);
        if (ability == null)
        {
            services.Logger.LogError($"{nameof(GetHeroAsync)} - Could not find ability for the hero with id {heroId}");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await WriteToResponseAsync(context, ability, services.ProtobufSerializer);
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
}
