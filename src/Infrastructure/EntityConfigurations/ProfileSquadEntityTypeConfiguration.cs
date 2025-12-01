using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class ProfileSquadEntityTypeConfiguration : IEntityTypeConfiguration<ProfileSquadEntity>
{
    public void Configure(EntityTypeBuilder<ProfileSquadEntity> builder)
    {
        builder.ToTable("profile_squads");

        builder.HasKey(p => p.Id);

        builder.Ignore(p => p.Key);

        builder.Property(p => p.UnitId).IsRequired();
        builder.Property(p => p.AbilityLevel).IsRequired();
        builder.Property(p => p.AscensionLevel).IsRequired();
        builder.Property(p => p.AwakeningLevel).IsRequired();
        builder.Property(p => p.Level).IsRequired();
        builder.Property(p => p.CollectedAt).IsRequired();
        builder.Property(p => p.Age).IsRequired();

        builder.HasIndex(p => p.UnitId);
        builder.HasIndex(p => p.Age);
        builder.HasIndex(p => p.Level);
        builder.HasIndex(p => p.CollectedAt).IsDescending();
        builder.HasIndex(p => new {p.PlayerId, p.UnitId, p.CollectedAt}).IsUnique();

        builder.HasOne(x => x.Data).WithOne(x => x.ProfileSquad)
            .HasForeignKey<ProfileSquadDataEntity>(x => x.ProfileSquadId).IsRequired();
    }
}
