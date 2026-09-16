using DoseRegistry.Api.Data;
using DoseRegistry.Api.Dtos;
using DoseRegistry.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoseRegistry.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class WorkersController : ControllerBase
{
    private readonly DoseRegistryDbContext _db;

    public WorkersController(DoseRegistryDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<WorkerDto>>> GetAll()
    {
        var workers = await _db.Workers
            .OrderBy(w => w.FullName)
            .Select(w => ToDto(w))
            .ToListAsync();

        return Ok(workers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkerDto>> GetById(int id)
    {
        var worker = await _db.Workers.FindAsync(id);
        if (worker is null)
        {
            return NotFound();
        }

        return Ok(ToDto(worker));
    }

    [HttpPost]
    public async Task<ActionResult<WorkerDto>> Create([FromBody] CreateWorkerDto dto)
    {
        var worker = new Worker
        {
            FullName = dto.FullName.Trim(),
            PersonalNumber = dto.PersonalNumber.Trim()
        };

        if (string.IsNullOrEmpty(worker.FullName))
        {
            return BadRequest(new { error = "The name is empty." });
        }

        bool exists = await _db.Workers.AnyAsync(w => w.PersonalNumber.ToLowerInvariant() == dto.PersonalNumber.ToLowerInvariant());
        if (exists)
        {
            return Conflict(new { error = $"The worker with the personal number '{dto.PersonalNumber}' already exists." });
        }

        _db.Workers.Add(worker);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = worker.Id }, ToDto(worker));
    }

    private static WorkerDto ToDto(Worker worker) => new()
    {
        Id = worker.Id,
        FullName = worker.FullName,
        PersonalNumber = worker.PersonalNumber
    };
}
