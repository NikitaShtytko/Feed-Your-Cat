namespace FeedYourCat.Entities
{
    public class FeederLog
    {
        public int Id { get; set; }
        public int Feeder_Id { get; set; }
        public string Action { get; set; }
        public string Date { get; set; }
    }
}