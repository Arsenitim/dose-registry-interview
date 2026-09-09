using DoseRegistry.Api.Data;
using DoseRegistry.Api.Dtos;
using DoseRegistry.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoseRegistry.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class DoseRecordsController : ControllerBase
{
    private readonly DoseRegistryDbContext _db;

    public DoseRecordsController(DoseRegistryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<DoseRecordDto>>> GetAll([FromQuery] int? workerId)
    {
        var query = _db.DoseRecords.AsQueryable();
        if (workerId is not null)
        {
            query = query.Where(r => r.WorkerId == workerId);
        }

        var records = await query
            .OrderBy(r => r.PeriodStart)
            .Select(r => ToDto(r))
            .ToListAsync();

        return Ok(records);
    }

    [HttpPost]
    public async Task<ActionResult<DoseRecordDto>> Create([FromBody] CreateDoseRecordDto dto)
    {
        var workerExists = await _db.Workers.AnyAsync(w => w.Id == dto.WorkerId);
        if (!workerExists)
        {
            return NotFound(new { error = $"Worker {dto.WorkerId} does not exist." });
        }

        if (dto.PeriodEnd < dto.PeriodStart)
        {
            return BadRequest(new { error = "periodEnd must not be before periodStart." });
        }

        var record = new DoseRecord
        {
            WorkerId = dto.WorkerId,
            PeriodStart = dto.PeriodStart,
            PeriodEnd = dto.PeriodEnd,
            DoseValueMsv = dto.DoseValueMsv
        };

        _db.DoseRecords.Add(record);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { workerId = record.WorkerId }, ToDto(record));
    }

    private static DoseRecordDto ToDto(DoseRecord record) => new()
    {
        Id = record.Id,
        WorkerId = record.WorkerId,
        PeriodStart = record.PeriodStart,
        PeriodEnd = record.PeriodEnd,
        DoseValueMsv = record.DoseValueMsv
    };
}
