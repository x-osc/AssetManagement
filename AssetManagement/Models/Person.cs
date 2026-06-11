using AssetManagement.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetManagement.Models
{
    [Index(nameof(AcNumber), IsUnique = true)]
    public class Person
    {
        public int Id { get; set; }
        [MaxLength(8)]
        public string AcNumber { get; set; }
        public string Name { get; set; }
        public PersonRole Role { get; set; }

        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }
    }

    public enum PersonRole
    {
        Teacher,
        Student,
        ITSupport
    }
}
