using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class AllianceRankingEntityTypeConfiguration : IEntityTypeConfiguration<AllianceRanking>
{
    public void Configure(EntityTypeBuilder<AllianceRanking> builder)
    {
        builder.ToTable("alliance_rankings");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Type).IsRequired();
        builder.Property(p => p.Points).IsRequired();
        builder.Property(p => p.Rank).IsRequired();
        builder.Property(p => p.CollectedAt).IsRequired();

        builder.HasIndex(p => p.CollectedAt).IsDescending();
        builder.HasIndex(p => new {p.Type, p.CollectedAt})
            .IsDescending(false, false);
        builder.HasIndex(p => new {p.AllianceId, p.Type, p.CollectedAt})
            .IsDescending(false, false, false);
    }
}