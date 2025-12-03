using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PvpBattleTeamsEntityTypeConfiguration : IEntityTypeConfiguration<PvpBattleTeams>
{
    public void Configure(EntityTypeBuilder<PvpBattleTeams> builder)
    {
        builder.ToTable("pvp_battle_teams");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.WinnerTeam).IsRequired().HasConversion<StringCompressionConverter>();
        builder.Property(p => p.LoserTeam).IsRequired().HasConversion<StringCompressionConverter>();
    }
}
