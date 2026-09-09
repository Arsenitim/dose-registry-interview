namespace DoseRegistry.Api.Dtos;

public class DoseRecordDto
{
    public int Id { get; set; }
    public int WorkerId { get; set; }
    public DateOnly PeriodStart { get; set; }
    public DateOnly PeriodEnd { get; set; }
    public decimal DoseValueMsv { get; set; }
}
