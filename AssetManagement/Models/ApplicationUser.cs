using Microsoft.AspNetCore.Identity;

namespace AssetManagement.Models { 
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }
        public Person Person { get; set; }
    }
}
