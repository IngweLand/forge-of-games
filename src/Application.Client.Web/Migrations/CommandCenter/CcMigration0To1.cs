using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Migrations.CommandCenter;

public class CcMigration0To1 : IMigration<BasicCommandCenterProfile>
{
    public int FromVersion => 0;
    public int ToVersion => 1;

    public BasicCommandCenterProfile Migrate(BasicCommandCenterProfile model)
    {
        var heroesDic = model.Heroes.ToDictionary(x => x.Id!, x => x.HeroId);
        var teams = model.Teams.Select(team =>
            new CommandCenterProfileTeam
            {
                Id = team.Id,
                Name = team.Name,
                HeroIds = team.HeroProfileIds.Select(x => heroesDic[x]).ToHashSet(),
            }).ToList();
        return new BasicCommandCenterProfile
        {
            Id = model.Id,
            Name = model.Name,
            BarracksProfile = model.BarracksProfile,
            Heroes = model.Heroes,
            Teams = teams,
            CommandCenterVersion = model.CommandCenterVersion,
            SchemaVersion = ToVersion,
        };
    }
}
