using System.ComponentModel.DataAnnotations;

namespace FeedYourCat.Models.Feeders
{
    public class NewFeederModel
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Name { get; set; }
    }
}