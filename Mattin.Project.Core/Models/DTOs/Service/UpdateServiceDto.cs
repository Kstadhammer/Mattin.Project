using System.ComponentModel.DataAnnotations;
using Mattin.Project.Core.Models.DTOs.Base;

namespace Mattin.Project.Core.Models.DTOs.Service;

public class UpdateServiceDto : BaseDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Base price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Base price must be greater than or equal to 0")]
    public decimal BasePrice { get; set; }

    [Required(ErrorMessage = "Hourly rate is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Hourly rate must be greater than or equal to 0")]
    public decimal HourlyRate { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [StringLength(50, ErrorMessage = "Category cannot be longer than 50 characters")]
    public string? Category { get; set; }
}
