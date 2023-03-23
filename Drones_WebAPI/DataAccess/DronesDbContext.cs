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
        public DbSet<Drone> Drones { get; set; }
        public DbSet<Medication> Medications { get; set; }
    }
}
