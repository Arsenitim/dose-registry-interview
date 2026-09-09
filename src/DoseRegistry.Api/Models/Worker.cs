namespace DoseRegistry.Api.Models;

public class Worker
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PersonalNumber { get; set; } = string.Empty;

    public List<DoseRecord> DoseRecords { get; set; } = new();
}
