using DoseRegistry.Api.Dtos;

namespace DoseRegistry.Api.Services;

public interface IAnnualSummaryReportService
{
    Task<List<AnnualSummaryRowDto>> GetAnnualSummaryAsync(int year);
}
