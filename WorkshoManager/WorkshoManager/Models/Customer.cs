using System.ComponentModel.DataAnnotations;

namespace WorkshoManager.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new();

    }
}
