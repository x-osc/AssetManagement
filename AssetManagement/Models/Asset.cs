using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public AssetStatus Status { get; set; }
        [DataType(DataType.Date)]
        public DateTime? PurchaseDate { get; set; }
        public string Notes { get; set; } = "";

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<MaintenanceAssignment> MaintenanceLogs { get; set; } = new List<MaintenanceAssignment>();
    }

    public enum AssetStatus
    {
        Available,
        Assigned,
        Maintenance,
        Retired
    }
}
