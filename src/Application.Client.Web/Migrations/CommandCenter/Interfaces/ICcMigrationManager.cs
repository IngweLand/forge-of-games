using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Migrations.CommandCenter.Interfaces;

public interface ICcMigrationManager
{
    BasicCommandCenterProfile Migrate(BasicCommandCenterProfile model);
}
