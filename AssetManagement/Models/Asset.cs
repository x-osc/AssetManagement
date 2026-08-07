using AssetManagement.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Models
{
    public class Asset
    {
        public int Id { get; set; }

        [Display(Name = "Serial Number")]
        [StringLength(50)]
        public string SerialNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Purchase Date")]
        public DateTime? PurchaseDate { get; set; } 

        public AssetStatus Status { get; set; } = AssetStatus.Available;

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Notes { get; set; } = "";
            
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<MaintenanceAssignment> MaintenanceLogs { get; set; } = new List<MaintenanceAssignment>();

        [Display(Name = "Maintenance Status")]
        public MaintenanceStatus? MaintenanceStatus
        {
            get
            {
                var current = MaintenanceLogs
                    .FirstOrDefault(m => m.CompletedAt == null);

                return current?.GetStatus();
            }
        }

        [Display(Name = "Maintenance Assignment")]
        public MaintenanceAssignment? CurrentMaintenance
        {
            get
            {
                return MaintenanceLogs.FirstOrDefault(m => m.CompletedAt == null);
            }
        }

        [Display(Name = "Assigned To")]
        public Assignment? CurrentAssignment
        {
            get
            {
                return Assignments.FirstOrDefault(a => a.ReturnedAt == null);
            }
        }
    }

    public enum AssetStatus
    {
        Available,
        Retired
    }
}
