﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkshoManager.Models;
namespace WorkshoManager.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<UsedPart> UsedParts { get; set; }
    public DbSet<ServiceTask> ServiceTasks { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Customer)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Vehicle)
            .WithMany(v => v.Orders)
            .HasForeignKey(o => o.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Mechanic)
            .WithMany()
            .HasForeignKey(o => o.MechanicId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<UsedPart>()
            .HasOne(up => up.Part)
            .WithMany()
            .HasForeignKey(up => up.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UsedPart>()
            .HasOne(up => up.ServiceTask)
            .WithMany()
            .HasForeignKey(up => up.ServiceTaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }



}
