using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Models
{
    public class MaintenanceAssignment
    {
        public int Id { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? Notes { get; set; } = "";

        public int AssetId { get; set; }
        public Asset Asset { get; set; }
        public int TechnicianId { get; set; }
        [ForeignKey(nameof(TechnicianId))]
        public Person Technician { get; set; }
    }
}
