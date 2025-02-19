using System.ComponentModel.DataAnnotations;
using Mattin.Project.Core.Models.DTOs.Base;

namespace Mattin.Project.Core.Models.DTOs.Client;

public class UpdateClientDto : BaseDto
{
    [Required(ErrorMessage = "Namn är obligatoriskt")]
    [StringLength(100, ErrorMessage = "Namnet får inte vara längre än 100 tecken")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "E-post är obligatorisk")]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Telefonnummer är obligatoriskt")]
    [Phone(ErrorMessage = "Ogiltigt telefonnummer")]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(200, ErrorMessage = "Adressen får inte vara längre än 200 tecken")]
    public string? Address { get; set; }
}
