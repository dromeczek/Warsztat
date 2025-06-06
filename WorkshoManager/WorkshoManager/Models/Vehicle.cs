using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkshoManager.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        [StringLength(17, MinimumLength = 17)]
        public string VIN { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }

        public int CustomerId { get; set; }
        public string? ImageUrl { get; set; }

        [ValidateNever]
        public Customer Customer { get; set; }

        public List<Order> Orders { get; set; } = new(); 
    }
}
