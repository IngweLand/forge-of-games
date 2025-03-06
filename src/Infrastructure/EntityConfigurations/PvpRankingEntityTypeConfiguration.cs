using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PvpRankingEntityTypeConfiguration : IEntityTypeConfiguration<PvpRanking>
{
    public void Configure(EntityTypeBuilder<PvpRanking> builder)
    {
        builder.ToTable("pvp_rankings");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Points).IsRequired();
        builder.Property(p => p.Rank).IsRequired();
        builder.Property(p => p.CollectedAt).IsRequired();

        builder.HasIndex(p => p.CollectedAt).IsDescending();
    }
}