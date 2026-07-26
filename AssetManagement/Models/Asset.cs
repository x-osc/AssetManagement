using AssetManagement.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PurchaseDate { get; set; }

        public AssetStatus Status { get; set; } = AssetStatus.Available;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Notes { get; set; } = "";
            
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<MaintenanceAssignment> MaintenanceLogs { get; set; } = new List<MaintenanceAssignment>();

        public MaintenanceStatus? GetStatus()
        {
            var current = MaintenanceLogs
                .FirstOrDefault(m => m.CompletedAt == null);

            return current?.GetStatus();
        }

        public Assignment? GetAssignment()
        {
            return Assignments.FirstOrDefault(a => a.ReturnedAt == null);
        }
    }

    public enum AssetStatus
    {
        Available,
        Retired
    }

    public class AssetFilter
    {
        public string? Search { get; set; }
        public AssetStatus? Status { get; set; }
        public string? Sort { get; set; }
        public string? Order { get; set; }
    }
}
