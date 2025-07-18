using Ingweland.Fog.Application.Client.Web.Migrations.CommandCenter.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Migrations.CommandCenter;

public class CcMigrationManager : ICcMigrationManager
{
    private readonly List<IMigration<BasicCommandCenterProfile>> _migrations = [new CcMigration0To1()];

    public BasicCommandCenterProfile Migrate(BasicCommandCenterProfile model)
    {
        var currentVersion = model.SchemaVersion;
        var targetVersion = _migrations.Max(m => m.ToVersion);
        var currentModel = model;
        while (currentVersion < targetVersion)
        {
            var migration = _migrations.FirstOrDefault(m => m.FromVersion == currentVersion);
            if (migration == null)
            {
                throw new InvalidOperationException($"No migration found from version {currentVersion}");
            }

            currentModel = migration.Migrate(model);
            currentVersion = currentModel.SchemaVersion;
        }

        return currentModel;
    }
}
