using DoseRegistry.Api.Dtos;
using DoseRegistry.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoseRegistry.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IAnnualSummaryReportService _reportService;

    public ReportsController(IAnnualSummaryReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("annual-summary")]
    public async Task<ActionResult<List<AnnualSummaryRowDto>>> GetAnnualSummary([FromQuery] int year)
    {
        if (year is < 1900 or > 2200)
        {
            return BadRequest(new { error = "year must be a plausible calendar year." });
        }

        return Ok(await _reportService.GetAnnualSummaryAsync(year));
    }
}
