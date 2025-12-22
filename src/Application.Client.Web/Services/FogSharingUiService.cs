using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Shared.Helpers.Interfaces;

namespace Ingweland.Fog.Application.Client.Web.Services;

public class FogSharingUiService(
    IProtobufSerializer protobufSerializer,
    IFogSharingService sharingService,
    IPersistenceService persistenceService)
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

    public async Task<bool> LoadCityStrategyAsync(string shareId)
    {
        var data = await sharingService.GetAsync(shareId);
        if (data == null)
        {
            return false;
        }

        try
        {
            var strategy = protobufSerializer.DeserializeFromBytes<CityStrategy>(Convert.FromBase64String(data.Data));
            strategy.Id = Guid.NewGuid().ToString();
            strategy.UpdatedAt = DateTime.Now;
            await persistenceService.SaveCityStrategy(strategy);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
