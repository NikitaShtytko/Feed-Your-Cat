using System;
using Newtonsoft.Json.Linq;

namespace FeedYourCat.Models.Feeders
{
    public class FeederModel
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string Type { get; set; }
        public int State { get; set; }
        public bool Status { get; set; }
        public bool Empty { get; set; }
        public JsonObject Schedule_Feed { get; set; }
    }
}