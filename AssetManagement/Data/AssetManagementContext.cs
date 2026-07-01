using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AssetManagement.Data
{
    public class AssetManagementContext : IdentityDbContext
    {
        public AssetManagementContext (DbContextOptions<AssetManagementContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Assignment>().HasIndex(a => a.AssetId).HasFilter("[ReturnedAt] IS NULL").IsUnique();

            modelBuilder.Entity<Assignment>().ToTable(t => t.HasCheckConstraint(
                "CK_Assignment_PersonOrLocation", 
                "(PersonId IS NOT NULL AND LocationId IS NULL) OR (LocationId IS NOT NULL AND PersonId IS NULL)"
            ));
        }

        public DbSet<AssetManagement.Models.Asset> Asset { get; set; } = default!;
        public DbSet<AssetManagement.Models.Assignment> Assignment { get; set; } = default!;
        public DbSet<AssetManagement.Models.Category> Category { get; set; } = default!;
        public DbSet<AssetManagement.Models.Location> Location { get; set; } = default!;
        public DbSet<AssetManagement.Models.MaintenanceAssignment> MaintenanceAssignment { get; set; } = default!;
    }
}
