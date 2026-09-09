using System.ComponentModel.DataAnnotations;

namespace DoseRegistry.Api.Dtos;

public class CreateWorkerDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string PersonalNumber { get; set; } = string.Empty;
}
