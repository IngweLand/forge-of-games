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
    }
}