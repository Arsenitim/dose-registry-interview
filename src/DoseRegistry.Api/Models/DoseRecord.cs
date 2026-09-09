namespace DoseRegistry.Api.Models;

public class DoseRecord
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public Worker Worker { get; set; } = null!;

    public DateOnly PeriodStart { get; set; }
    public DateOnly PeriodEnd { get; set; }
    public decimal DoseValueMsv { get; set; }
}
