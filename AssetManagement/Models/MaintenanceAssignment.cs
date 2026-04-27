namespace AssetManagement.Models
{
    public class MaintenanceAssignment
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int TechnicianId { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Notes { get; set; } = "";

        public Asset Asset { get; set; }
        public Person Technician { get; set; }
    }
}
