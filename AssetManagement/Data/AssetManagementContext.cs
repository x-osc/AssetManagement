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

        public DbSet<AssetManagement.Models.Asset> Asset { get; set; } = default!;
    }
}
