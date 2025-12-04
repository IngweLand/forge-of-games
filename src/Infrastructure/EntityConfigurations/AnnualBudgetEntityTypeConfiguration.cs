using Ingweland.Fog.Models.Fog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ingweland.Fog.Infrastructure.EntityConfigurations;

public class AnnualBudgetEntityTypeConfiguration : IEntityTypeConfiguration<AnnualBudget>
{
    public void Configure(EntityTypeBuilder<AnnualBudget> builder)
    {
        builder.ToTable("annual_budgets");

        builder.HasKey(x => x.Year);

        builder.Property(x => x.Year).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.ServerGoal).IsRequired();
    }
}
