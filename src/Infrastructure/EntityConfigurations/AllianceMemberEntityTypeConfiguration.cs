using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class AllianceMemberEntityTypeConfiguration : IEntityTypeConfiguration<AllianceMemberEntity>
{
    public void Configure(EntityTypeBuilder<AllianceMemberEntity> builder)
    {
        builder.ToTable("alliance_members");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Role).HasConversion<string>();

        builder.HasIndex(p => p.Role);
    }
}
