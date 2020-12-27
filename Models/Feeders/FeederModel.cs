using System;
using Newtonsoft.Json.Linq;

namespace FeedYourCat.Models.Feeders
{
    public class FeederModel
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string Type { get; set; }
        public bool Is_Empty { get; set; }
        public int Fullness { get; set; }
        public string Name { get; set; }
    }
}