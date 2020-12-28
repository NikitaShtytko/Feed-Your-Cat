using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using FeedYourCat.Entities;

namespace FeedYourCat.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer("");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Feeder> Feeders { get; set; }
        public DbSet<Tag> Feeder_Tags { get; set; }
        public DbSet<Schedule> Feeder_Schedules { get; set; }
    }
}