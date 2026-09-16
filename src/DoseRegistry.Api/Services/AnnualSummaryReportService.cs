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
        return await _db.DoseRecords
            .Where(r => r.PeriodStart.Year == year)
            .GroupBy(r => new { r.WorkerId, r.Worker.FullName, r.Worker.PersonalNumber })
            .Select(g => new AnnualSummaryRowDto
            {
                WorkerId = g.Key.WorkerId,
                WorkerFullName = g.Key.FullName,
                WorkerPersonalNumber = g.Key.PersonalNumber,
                Year = year,
                TotalDoseMsv = g.Sum(r => r.DoseValueMsv)
            })
            .OrderBy(r => r.WorkerFullName)
            .ToListAsync();
    }
}
