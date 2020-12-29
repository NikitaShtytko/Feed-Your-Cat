namespace FeedYourCat.Entities
{
    public class UserLog
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string Action { get; set; }
        public string Date { get; set; }
    }
}