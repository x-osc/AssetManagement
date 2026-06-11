using AssetManagement.Models;
using Microsoft.AspNetCore.Identity;
namespace AssetManagement.Data;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public Person Person { get; set; }
}
