using Microsoft.AspNetCore.Identity;

namespace AssetManagement.Models { 
    public class ApplicationUser : IdentityUser
    {
        // Id is inherited
        public Person Person { get; set; }
    }
}
