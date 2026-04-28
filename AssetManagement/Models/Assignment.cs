using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace AssetManagement.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int? PersonId { get; set; }
        public int? LocationId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; }

        public Asset Asset { get; set; }
        public Person? Person { get; set; }
        public Location? Location { get; set; }
    }
}
