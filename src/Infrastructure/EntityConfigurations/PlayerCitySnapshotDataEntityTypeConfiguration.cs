using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class PlayerCitySnapshotDataEntityTypeConfiguration : IEntityTypeConfiguration<PlayerCitySnapshotDataEntity>
{
    public void Configure(EntityTypeBuilder<PlayerCitySnapshotDataEntity> builder)
    {
        builder.ToTable("player_city_snapshot_data");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Data).IsRequired()
            .HasConversion<BytearrayCompressionConverter>(new ByteArrayValueComparer());
    }
}
