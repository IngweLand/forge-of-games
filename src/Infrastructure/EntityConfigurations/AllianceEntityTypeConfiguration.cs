using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class AllianceEntityTypeConfiguration : IEntityTypeConfiguration<Alliance>
{
    public void Configure(EntityTypeBuilder<Alliance> builder)
    {
        builder.ToTable("alliances");

        builder.HasKey(p => new {p.WorldId, p.InGameAllianceId});

        builder.Ignore(p => p.Key);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.WorldId).IsRequired().HasMaxLength(48);
        builder.Property(p => p.UpdatedAt).IsRequired();
        builder.Property(p => p.Rank).IsRequired();
        builder.Property(p => p.RankingPoints).IsRequired();
        builder.Property(p => p.MemberCount).IsRequired();

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.WorldId);
        builder.HasIndex(p => p.InGameAllianceId);
        builder.HasIndex(p => p.RankingPoints).IsDescending();

        builder.HasMany(p => p.Rankings).WithOne().HasForeignKey(p => new {p.WorldId, p.InGameAllianceId});
    }
}