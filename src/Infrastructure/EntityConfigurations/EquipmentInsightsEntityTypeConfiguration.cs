using Ingweland.Fog.Infrastructure.Converters;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class EquipmentInsightsEntityTypeConfiguration : IEntityTypeConfiguration<EquipmentInsightsEntity>
{
    public void Configure(EntityTypeBuilder<EquipmentInsightsEntity> builder)
    {
        builder.ToTable("equipment_insights");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.UnitId).IsRequired();
        builder.Property(p => p.FromLevel).IsRequired();
        builder.Property(p => p.ToLevel).IsRequired();
        builder.Property(p => p.EquipmentSlotType).IsRequired().HasConversion<string>();
        builder.Property(p => p.EquipmentSets).IsRequired()
            .HasConversion<JsonValueWithStringEnumConverter<ICollection<EquipmentSet>>>(
                new CollectionValueComparer<EquipmentSet>());
        builder.Property(p => p.MainAttributes).IsRequired()
            .HasConversion<JsonValueWithStringEnumConverter<ICollection<StatAttribute>>>(
                new CollectionValueComparer<StatAttribute>());
        builder.Property(p => p.SubAttributesLevel4)
            .HasConversion<JsonValueWithStringEnumConverter<ICollection<StatAttribute>>>(
                new CollectionValueComparer<StatAttribute>());
        builder.Property(p => p.SubAttributesLevel8)
            .HasConversion<JsonValueWithStringEnumConverter<ICollection<StatAttribute>>>(
                new CollectionValueComparer<StatAttribute>());
        builder.Property(p => p.SubAttributesLevel12)
            .HasConversion<JsonValueWithStringEnumConverter<ICollection<StatAttribute>>>(
                new CollectionValueComparer<StatAttribute>());
        builder.Property(p => p.SubAttributesLevel16)
            .HasConversion<JsonValueWithStringEnumConverter<ICollection<StatAttribute>>>(
                new CollectionValueComparer<StatAttribute>());

        builder.HasIndex(p => p.UnitId);
        builder.HasIndex(p => p.FromLevel);
        builder.HasIndex(p => p.ToLevel);
        builder.HasIndex(p => p.EquipmentSlotType);
        builder.HasIndex(p => new {p.UnitId, p.EquipmentSlotType, p.FromLevel, p.ToLevel}).IsUnique();
    }
}
