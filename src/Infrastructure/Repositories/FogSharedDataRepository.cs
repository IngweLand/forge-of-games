using Ingweland.Fog.Application.Server.Interfaces;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class FogSharedDataRepository(string connectionString, string containerName)
    : BinaryDataStorageRepository(connectionString, containerName), IFogSharedDataStorageRepository
{
}
