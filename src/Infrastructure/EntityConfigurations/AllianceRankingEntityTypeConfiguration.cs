using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class AllianceRankingEntityTypeConfiguration : IEntityTypeConfiguration<AllianceRanking>
{
    public void Configure(EntityTypeBuilder<AllianceRanking> builder)
    {
        builder.ToTable("alliance_rankings");

        builder.HasKey(p => new {p.WorldId, p.InGameAllianceId, p.CollectedAt});
        
        builder.Ignore(p => p.Key);
        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Type).IsRequired();
        builder.Property(p => p.Points).IsRequired();
        builder.Property(p => p.Rank).IsRequired();
        builder.Property(p => p.WorldId).IsRequired().HasMaxLength(48);
        builder.Property(p => p.CollectedAt).IsRequired();
        builder.Property(p => p.Type).IsRequired();
        builder.Property(p => p.MemberCount).IsRequired();

        builder.HasIndex(p => new {p.WorldId, p.InGameAllianceId});
        builder.HasIndex(p => p.CollectedAt).IsDescending();
        builder.HasIndex(p => p.WorldId);
    }
}