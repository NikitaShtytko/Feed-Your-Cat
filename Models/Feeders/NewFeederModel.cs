using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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