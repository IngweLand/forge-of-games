using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Migrations;

public interface IMigration<T> where T : VersionedModel
{
    int FromVersion { get; }
    int ToVersion { get; }

    T Migrate(T model);
}

