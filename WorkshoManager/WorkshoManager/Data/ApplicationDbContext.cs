using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkshoManager.Models;
namespace WorkshoManager.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Customer> Customers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
