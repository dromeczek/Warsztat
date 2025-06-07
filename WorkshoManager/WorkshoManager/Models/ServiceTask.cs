using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WorkshoManager.Models
{
    public class ServiceTask
    {
        public int Id { get; set; }
        public List<UsedPart> UsedParts { get; set; }

        [Required]
        [Display(Name = "Nazwa czynności")]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Koszt robocizny")]
        public decimal LaborCost { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        [ValidateNever] 
        public Order Order { get; set; }
    }
}
