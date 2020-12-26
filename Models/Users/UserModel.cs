namespace FeedYourCat.Models.Users
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "user";
        public byte Status { get; set; } = 0;
    }
}