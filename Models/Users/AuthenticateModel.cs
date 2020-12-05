using System.ComponentModel.DataAnnotations;

namespace FeedYourCat.Models.Users
{
    public class AuthenticateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}