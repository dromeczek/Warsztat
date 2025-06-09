using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using WorkshoManager.Models;

namespace WorkshoManager.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Treść")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Autor (IdentityUser)
        public string AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public IdentityUser Author { get; set; }

        // Zlecenie
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}