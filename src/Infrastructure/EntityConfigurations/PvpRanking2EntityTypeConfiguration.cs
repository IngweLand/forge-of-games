using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PvpRanking2EntityTypeConfiguration : IEntityTypeConfiguration<PvpRanking2>
{
    public void Configure(EntityTypeBuilder<PvpRanking2> builder)
    {
        builder.ToTable("pvp_rankings2");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Tier).IsRequired();
        builder.Property(p => p.CollectedAt).IsRequired();

        builder.HasIndex(p => p.CollectedAt).IsDescending();
    }
}
