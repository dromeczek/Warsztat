using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WorkshoManager.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    public int CustomerId { get; set; }
    [ValidateNever]
    public Customer Customer { get; set; }

    [Required]
    public int VehicleId { get; set; }
    [ValidateNever]
    public Vehicle Vehicle { get; set; }

    public string? MechanicId { get; set; }
    [ValidateNever]
    public IdentityUser? Mechanic { get; set; }

    [Required]
    public string Description { get; set; }

    public string Status { get; set; } = "Nowe";
    public List<ServiceTask> Tasks { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}