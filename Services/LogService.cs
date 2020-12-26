using FeedYourCat.Entities;
using FeedYourCat.Helpers;

namespace FeedYourCat.Services
{
    public interface ILogService
    {
        Log Create(Log log);
    }

    public class LogService : ILogService
    {
        private DataContext _context;

        public LogService(DataContext context)
        {
            _context = context;
        }

        public Log Create(Log log)
        {
            _context.Log.Add(log);
            _context.SaveChanges();
            return log;
        }
    }
}