using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class AllianceEntityTypeConfiguration : IEntityTypeConfiguration<Alliance>
{
    public void Configure(EntityTypeBuilder<Alliance> builder)
    {
        builder.ToTable("alliances");

        builder.HasKey(p => p.Id);

        builder.Ignore(p => p.Key);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.WorldId).IsRequired().HasMaxLength(48);
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.WorldId);
        builder.HasIndex(p => p.InGameAllianceId);
        builder.HasIndex(p => p.RankingPoints).IsDescending();
        builder.HasIndex(p => new {p.WorldId, p.InGameAllianceId}).IsUnique();

        builder.HasMany(p => p.Rankings).WithOne().HasForeignKey(p => p.AllianceId);
        builder.HasMany(p => p.NameHistory).WithOne().HasForeignKey(p => p.AllianceId);
        builder.HasMany(p => p.Members).WithOne(p => p.CurrentAlliance).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(p => p.MemberHistory).WithMany(p => p.AllianceHistory);
        builder.HasOne(p => p.Leader).WithOne(p => p.LedAlliance).HasForeignKey<Alliance>(a => a.LeaderId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}