using DoseRegistry.Api.Data;
using DoseRegistry.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DoseRegistry.Api.Services;

public class AnnualSummaryReportService : IAnnualSummaryReportService
{
    private readonly DoseRegistryDbContext _db;

    public AnnualSummaryReportService(DoseRegistryDbContext db)
    {
        _db = db;
    }

    public async Task<List<AnnualSummaryRowDto>> GetAnnualSummaryAsync(int year)
    {
        var records = await _db.DoseRecords
            .Include(r => r.Worker)
            .Where(r => r.PeriodStart.Year == year)
            .ToListAsync();

        return records
            .GroupBy(r => r.Worker)
            .Select(g => new AnnualSummaryRowDto
            {
                WorkerId = g.Key.Id,
                WorkerFullName = g.Key.FullName,
                WorkerPersonalNumber = g.Key.PersonalNumber,
                Year = year,
                TotalDoseMsv = g.Sum(r => r.DoseValueMsv)
            })
            .OrderBy(r => r.WorkerFullName)
            .ToList();
    }
}
