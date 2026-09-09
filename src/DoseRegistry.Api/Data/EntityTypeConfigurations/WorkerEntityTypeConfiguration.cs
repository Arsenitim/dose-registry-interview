using DoseRegistry.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoseRegistry.Api.Data.EntityTypeConfigurations;

public class WorkerEntityTypeConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable("workers");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasColumnName("id");
        builder.Property(w => w.FullName).HasColumnName("full_name").IsRequired();
        builder.Property(w => w.PersonalNumber).HasColumnName("personal_number").IsRequired();

        builder.HasIndex(w => w.PersonalNumber).IsUnique();
    }
}
