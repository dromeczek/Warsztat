using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WorkshoManager.Models
{
    public class UsedPart
    {
        public int Id { get; set; }

        [ValidateNever]
        public int PartId { get; set; }

        [ValidateNever]
        public Part Part { get; set; }

        public int Quantity { get; set; }

        [ValidateNever]
        public int ServiceTaskId { get; set; }
        [ValidateNever]
        public ServiceTask ServiceTask { get; set; }
    }
}
