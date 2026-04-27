namespace AssetManagement.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string AcNumber { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public ApplicationUser? User { get; set; }
    }

    public enum PersonRole
    {
        Teacher,
        Student,
        ITSupport
    }
}
