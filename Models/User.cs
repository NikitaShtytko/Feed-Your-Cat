using Microsoft.AspNetCore.Identity;

namespace FeedYourCat.Models
{
    public class User : IdentityUser
    { 
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}