using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Fog.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class ProfileSquadDataEntityTypeConfiguration : IEntityTypeConfiguration<ProfileSquadDataEntity>
{
    public void Configure(EntityTypeBuilder<ProfileSquadDataEntity> builder)
    {
        builder.ToTable("profile_squad_data");

        builder.HasKey(p => p.Id);
        
        builder.Ignore(p => p.Hero);
        builder.Ignore(p => p.SupportUnit);

        builder.Property(p => p.SerializedHero).IsRequired();
        builder.Property(p => p.SerializedSupportUnit).IsRequired();
    }
}
