using System.ComponentModel.DataAnnotations;

namespace FeedYourCat.Models.Users
{
    public class AuthenticateModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}