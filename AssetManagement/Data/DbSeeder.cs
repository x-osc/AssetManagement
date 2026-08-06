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
                    new Asset() { SerialNumber = "004", Name = "HP Elitebook 840", Category = categories[0], Status = AssetStatus.Retired },

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
                        Asset = assets[0],
                        Person = people[2],
                        AssignedAt = new DateTime(2024, 3, 15),
                        ReturnedAt = new DateTime(2025, 1, 2),
                    },
                    new Assignment()
                    {
                        Asset = assets[0],
                        Person = people[0],
                        AssignedAt = new DateTime(2025, 2, 15),
                        Notes = "Assigned for teaching duties"
                    },
                    new Assignment()
                    {
                        Asset = assets[1],
                        Person = people[1],
                        AssignedAt = new DateTime(2025, 1, 15),
                    },
                    new Assignment()
                    {
                        Asset = assets[2],
                        Person = people[2],
                        AssignedAt = new DateTime(2025, 1, 15),
                    },
                    new Assignment()
                    {
                        Asset = assets[3],
                        Person = people[0],
                        AssignedAt = new DateTime(2025, 1, 15),
                        ReturnedAt = new DateTime(2026, 1, 15),
                    },
                    new Assignment() {
                        Asset = assets[7],
                        Location = locations[0],
                        AssignedAt = new DateTime(2025, 2, 1)
                    }
                };
                context.AddRange(assignments);

                context.SaveChanges();

                var maintenanceLogs = new List<MaintenanceAssignment>
                {
                    new MaintenanceAssignment()
                    {
                        Asset = assets[0],
                        Technician = people[0],
                        Notes = "Replaced battery",
                        StartedAt = new DateTime(2025, 6, 1),
                        CompletedAt = new DateTime(2025, 6, 1)
                    },
                    new MaintenanceAssignment()
                    {
                        Asset = assets[1],
                        Technician = people[0],
                        StartedAt = new DateTime(2025, 7, 1),
                        Notes = "Annual hardware inspection",
                    },
                };
                context.AddRange(maintenanceLogs);

                context.SaveChanges();
            }
        }
    }
}
