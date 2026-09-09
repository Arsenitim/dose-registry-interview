namespace DoseRegistry.Api.Dtos;

public class AnnualSummaryRowDto
{
    public int WorkerId { get; set; }
    public string WorkerFullName { get; set; } = string.Empty;
    public string WorkerPersonalNumber { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal TotalDoseMsv { get; set; }
}
