using DoseRegistry.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DoseRegistry.Api.Data;

public class DoseRegistryDbContext : DbContext
{
    public DoseRegistryDbContext(DbContextOptions<DoseRegistryDbContext> options) : base(options)
    {
    }

    public DbSet<Worker> Workers => Set<Worker>();
    public DbSet<DoseRecord> DoseRecords => Set<DoseRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DoseRegistryDbContext).Assembly);
    }
}
