using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Models;

namespace AssetManagement.Data
{
    public class AssetManagementContext : DbContext
    {
        public AssetManagementContext (DbContextOptions<AssetManagementContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>().HasIndex(a => a.AssetId).HasFilter("[ReturnedAt] IS NULL").IsUnique();

            modelBuilder.Entity<Assignment>().ToTable(t => t.HasCheckConstraint(
                "CK_Assignment_PersonOrLocation", 
                "(PersonId IS NOT NULL AND LocationId IS NULL) OR (LocationId IS NOT NULL AND PersonId IS NULL)"
            ));
        }

        public DbSet<AssetManagement.Models.Asset> Asset { get; set; } = default!;
    }
}
