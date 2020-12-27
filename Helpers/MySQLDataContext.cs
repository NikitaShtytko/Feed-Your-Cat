using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FeedYourCat.Helpers
{
    public class MySqlDataContext : DataContext
    {
        public MySqlDataContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseMySql("server=localhost;port=3306;database=feeder;user=kolwot;password=qwerty123");
        }
    }
}