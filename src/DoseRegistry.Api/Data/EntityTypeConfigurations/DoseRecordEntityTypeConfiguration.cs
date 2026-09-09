using DoseRegistry.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoseRegistry.Api.Data.EntityTypeConfigurations;

public class DoseRecordEntityTypeConfiguration : IEntityTypeConfiguration<DoseRecord>
{
    public void Configure(EntityTypeBuilder<DoseRecord> builder)
    {
        builder.ToTable("dose_records");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.WorkerId).HasColumnName("worker_id");
        builder.Property(d => d.PeriodStart).HasColumnName("period_start");
        builder.Property(d => d.PeriodEnd).HasColumnName("period_end");
        builder.Property(d => d.DoseValueMsv).HasColumnName("dose_value_msv").HasColumnType("numeric(10,4)");

        builder.HasOne(d => d.Worker)
            .WithMany(w => w.DoseRecords)
            .HasForeignKey(d => d.WorkerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
