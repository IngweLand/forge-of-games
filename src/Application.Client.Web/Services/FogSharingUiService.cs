using Ingweland.Fog.Application.Client.Web.Analytics;
using Ingweland.Fog.Application.Client.Web.Analytics.Interfaces;
using Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Client.Web.Services;

public class FogSharingUiService(
    IProtobufSerializer protobufSerializer,
    IFogSharingService sharingService,
    IEquipmentProfilePersistenceService equipmentProfilePersistenceService,
    IEquipmentProfileFactory equipmentProfileFactory,
    IAnalyticsService analyticsService,
    ILogger<FogSharingUiService> logger)
    : IFogSharingUiService
{
    public Task<CreatedShareDto> ShareAsync(SharedDataDto data)
    {
        return sharingService.ShareAsync(data);
    }

    public SharedDataDto CreateSharedData(CityStrategy cityStrategy)
    {
        var bytes = protobufSerializer.SerializeToBytes(cityStrategy);
        return new SharedDataDto {Data = Convert.ToBase64String(bytes)};
    }

    public SharedDataDto CreateSharedData(HohCity city)
    {
        var bytes = protobufSerializer.SerializeToBytes(city);
        return new SharedDataDto {Data = Convert.ToBase64String(bytes)};
    }

    public async Task<HohCity?> FetchCityAsync(string shareId)
    {
        var parameters = new Dictionary<string, object>
        {
            {AnalyticsParams.SHARE_ID, shareId},
        };
        _ = analyticsService.TrackEvent(AnalyticsEvents.FETCH_CITY_INIT, parameters);

        var data = await sharingService.GetAsync(shareId);
        if (data == null)
        {
            _ = analyticsService.TrackEvent(AnalyticsEvents.FETCH_CITY_ERROR, parameters);
            return null;
        }

        try
        {
            var city = protobufSerializer.DeserializeFromBytes<HohCity>(Convert.FromBase64String(data.Data));
            _ = analyticsService.TrackEvent(AnalyticsEvents.FETCH_CITY_SUCCESS, parameters);
            return city;
        }
        catch (Exception e)
        {
            _ = analyticsService.TrackEvent(AnalyticsEvents.FETCH_CITY_ERROR, parameters);
            logger.LogError(e, "Failed to load city.");
        }

        return null;
    }

    public SharedDataDto CreateSharedData(EquipmentProfile equipmentProfile)
    {
        var bytes = protobufSerializer.SerializeToBytes(equipmentProfile);
        return new SharedDataDto {Data = Convert.ToBase64String(bytes)};
    }

    public async Task<CityStrategy?> FetchCityStrategyAsync(string shareId)
    {
        var parameters = new Dictionary<string, object>
        {
            {AnalyticsParams.SHARE_ID, shareId},
        };
        _ = analyticsService.TrackEvent(AnalyticsEvents.FETCH_CITY_STRATEGY_INIT, parameters);

        var data = await sharingService.GetAsync(shareId);
        if (data == null)
        {
            _ = analyticsService.TrackEvent(AnalyticsEvents.FETCH_CITY_STRATEGY_ERROR, parameters);
            return null;
        }

        try
        {
            var strategy = protobufSerializer.DeserializeFromBytes<CityStrategy>(Convert.FromBase64String(data.Data));
            _ = analyticsService.TrackEvent(AnalyticsEvents.FETCH_CITY_STRATEGY_SUCCESS, parameters);
            return strategy;
        }
        catch (Exception e)
        {
            _ = analyticsService.TrackEvent(AnalyticsEvents.FETCH_CITY_STRATEGY_ERROR, parameters);
            logger.LogError(e, "Failed to load city strategy.");
        }

        return null;
    }

    public async Task<bool> LoadEquipmentProfileAsync(string shareId)
    {
        var data = await sharingService.GetAsync(shareId);
        if (data == null)
        {
            return false;
        }

        try
        {
            var loadedProfile =
                protobufSerializer.DeserializeFromBytes<EquipmentProfile>(Convert.FromBase64String(data.Data));
            var profile = equipmentProfileFactory.Create(loadedProfile.Name, loadedProfile.Heroes, loadedProfile.Relics,
                loadedProfile.Equipment, loadedProfile.BarracksProfile, loadedProfile.Configurations);
            await equipmentProfilePersistenceService.SaveAsync(profile);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to load equipment profile.");
            return false;
        }
    }
}
