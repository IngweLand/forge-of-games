using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class SharedSubmissionIdEntityTypeConfiguration : IEntityTypeConfiguration<SharedSubmissionIdEntity>
{
    public void Configure(EntityTypeBuilder<SharedSubmissionIdEntity> builder)
    {
        builder.ToTable("shared_submission_ids");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.SharedId).IsRequired();
        builder.Property(p => p.SubmissionId).IsRequired();
        builder.Property(p => p.ExpiresAt).IsRequired();
        
        builder.HasIndex(p => p.ExpiresAt);
        builder.HasIndex(p => p.SharedId).IsUnique();
        builder.HasIndex(p => p.SubmissionId);
    }
}
