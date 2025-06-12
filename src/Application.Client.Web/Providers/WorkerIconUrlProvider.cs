using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers;

public class WorkerIconUrlProvider(IAssetUrlProvider assetUrlProvider) : IWorkerIconUrlProvider
{
    public string GetIcon(CityId cityId, WorkerType workerType = WorkerType.Undefined)
    {
        var id = $"icon_workers_city_{CityId.Capital.ToString().ToLowerInvariant()}";
        switch (cityId)
        {
            case CityId.Mayas_Tikal:
            case CityId.Mayas_ChichenItza:
            case CityId.Mayas_SayilPalace:
            {
                id = workerType == WorkerType.PriestMaya ? "icon_workers_priestmaya" : "icon_workers_city_mayas";
                break;
            }
            case CityId.Vikings:
            {
                if (workerType == WorkerType.SailorVikings)
                {
                    id = "icon_workers_sailorvikings";
                }

                break;
            }
        }

        return assetUrlProvider.GetHohIconUrl(id);
    }
}