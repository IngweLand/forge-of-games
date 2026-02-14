namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IAdSenseService
{
    ValueTask InitializeAd(string adSlotId);
    ValueTask ClearAd(string adSlotId);
}
