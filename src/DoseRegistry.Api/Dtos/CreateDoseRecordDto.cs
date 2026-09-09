using System.ComponentModel.DataAnnotations;

namespace DoseRegistry.Api.Dtos;

public class CreateDoseRecordDto
{
    [Required]
    public int WorkerId { get; set; }

    [Required]
    public DateOnly PeriodStart { get; set; }

    [Required]
    public DateOnly PeriodEnd { get; set; }

    [Required]
    public decimal DoseValueMsv { get; set; }
}
