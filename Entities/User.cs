namespace FeedYourCat.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = "user";
        public bool Is_Registered{ get; set; } = false;
        public string Remember_Token { get; set; }
    }
}