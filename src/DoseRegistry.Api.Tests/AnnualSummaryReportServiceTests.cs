using DoseRegistry.Api.Data;
using DoseRegistry.Api.Models;
using DoseRegistry.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DoseRegistry.Api.Tests;

public class AnnualSummaryReportServiceTests
{
    private static DoseRegistryDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<DoseRegistryDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var db = new DoseRegistryDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    [Fact]
    public async Task GetAnnualSummaryAsync_IncludesRecordsFullyWithinTheQueriedYear()
    {
        await using var db = CreateContext(nameof(GetAnnualSummaryAsync_IncludesRecordsFullyWithinTheQueriedYear));

        var worker = new Worker { FullName = "Test Worker", PersonalNumber = "T-0001" };
        db.Workers.Add(worker);
        db.DoseRecords.Add(new DoseRecord
        {
            Worker = worker,
            PeriodStart = new DateOnly(2026, 3, 1),
            PeriodEnd = new DateOnly(2026, 3, 31),
            DoseValueMsv = 5.0m
        });
        await db.SaveChangesAsync();

        var result = await new AnnualSummaryReportService(db).GetAnnualSummaryAsync(2026);

        Assert.Single(result);
        Assert.Equal(5.0m, result[0].TotalDoseMsv);
    }
}
