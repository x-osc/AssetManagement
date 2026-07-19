using AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AssetManagement.Data
{
    public class DbSeeder
    {
        public static void Initialize(IServiceProvider provider)
        {
            using (var context = new AssetManagementContext(
            provider.GetRequiredService<
                DbContextOptions<AssetManagementContext>>()))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                if (context.Asset.Any())
                {
                    return; // DB has been seeded
                }

                var categories = new List<Category>
                {
                    new Category() { Name = "Laptop" },
                    new Category() { Name = "Desktop" },
                    new Category() { Name = "Projector" },
                    new Category() { Name = "Tablet" },
                    new Category() { Name = "Printer" },
                };
                context.AddRange(categories);

                var locations = new List<Location>
                {
                    new Location() { Name = "A27" },
                    new Location() { Name = "A28" },
                    new Location() { Name = "Library" },
                    new Location() { Name = "IT Storage" },
                };
                context.AddRange(locations);

                context.SaveChanges();

                var people = new List<Person>
                {
                    new Person() { AcNumber = "psv", Name = "Mr V Prasad", Role = PersonRole.Teacher },
                    new Person() { AcNumber = "ac149055", Name = "Max McCulloch", Role = PersonRole.Student },
                    new Person() { AcNumber = "ac148031", Name = "Felix Wong", Role = PersonRole.Student },
                };
                context.AddRange(people);

                var assets = new List<Asset>
                {
                    new Asset() { SerialNumber = "001", Name = "HP Elitebook 840", Category = categories[0] }, // Laptop
                    new Asset() { SerialNumber = "002", Name = "HP Elitebook 840", Category = categories[0] },
                    new Asset() { SerialNumber = "003", Name = "HP Elitebook 840", Category = categories[0] },
                    new Asset() { SerialNumber = "004", Name = "HP Elitebook 840", Category = categories[0] },

                    new Asset() { SerialNumber = "001", Name = "HP ProOne 245 G10", Category = categories[1] }, // Desktop
                    new Asset() { SerialNumber = "002", Name = "HP ProOne 245 G10", Category = categories[1] },

                    new Asset() { SerialNumber = "01", Name = "Projector", Category = categories[2] }, // Projector

                    new Asset() { SerialNumber = "001", Name = "Apple iPad", Category = categories[3] }, // Tablet
                    new Asset() { SerialNumber = "002", Name = "Apple iPad", Category = categories[3] },

                    new Asset() { SerialNumber = "001", Name = "Brother HL-1210W", Category = categories[4] }, // Printer
                };
                context.AddRange(assets);

                context.SaveChanges();

                var assignments = new List<Assignment> {
                    new Assignment()
                    {
                        AssetId = assets[0].Id,
                        PersonId = people[0].Id,
                        AssignedAt = new DateTime(2025, 1, 15),
                        ReturnedAt = new DateTime(2026, 1, 15),
                        Notes = "Assigned for teaching duties"
                    },
                    new Assignment() {
                        AssetId = assets[1].Id,
                        LocationId = locations[0].Id,
                        AssignedAt = new DateTime(2025, 2, 1)
                    }
                };
                context.AddRange(assignments);

                var maintenanceLogs = new List<MaintenanceAssignment>
                {
                    new MaintenanceAssignment()
                    {
                        AssetId = assets[0].Id,
                        TechnicianId = people[0].Id,
                        Notes = "Replaced battery",
                        StartedAt = new DateTime(2025, 6, 1),
                        CompletedAt = new DateTime(2025, 6, 1)
                    },
                    new MaintenanceAssignment()
                    {
                        AssetId = assets[1].Id,
                        TechnicianId = people[0].Id,
                        StartedAt = DateTime.UtcNow.AddDays(14),
                        Notes = "Annual hardware inspection",
                    },
                };
                context.AddRange(maintenanceLogs);

                context.SaveChanges();
            }
        }
    }
}
