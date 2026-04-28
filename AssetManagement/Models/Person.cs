using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Models
{
    [Index(nameof(AcNumber), IsUnique = true)]
    public class Person
    {
        public int Id { get; set; }
        public string AcNumber { get; set; }
        public string Name { get; set; }
        public PersonRole Role { get; set; }

        public ApplicationUser? User { get; set; }
    }

    public enum PersonRole
    {
        Teacher,
        Student,
        ITSupport
    }
}
