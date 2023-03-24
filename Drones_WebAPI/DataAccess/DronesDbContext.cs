using Drones_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Drones_WebAPI.DataAccess
{
    public class DronesDbContext : DbContext
    {
        public DronesDbContext(DbContextOptions<DronesDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medication>().HasOne(d => d.Drone).WithMany(dc => dc.Medications);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Drone> Drones { get; set; }
        public DbSet<Medication> Medications { get; set; }
    }
}
