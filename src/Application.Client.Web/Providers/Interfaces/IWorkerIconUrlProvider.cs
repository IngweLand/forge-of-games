using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Providers.Interfaces;

public interface IWorkerIconUrlProvider
{
    string GetIcon(CityId cityId, WorkerType workerType = WorkerType.Undefined);
}