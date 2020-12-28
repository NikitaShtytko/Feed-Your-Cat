using System.Linq;
using FeedYourCat.Helpers;

namespace FeedYourCat.Services
{
    public interface IValidationService
    {
        bool ValidateRole(string token, string role);
        int ValidateUserId(string token);
        bool ValidateUserFeeder(int feeder_id, int user_id);
        bool ValidateTag(int tagId, int userId);
        bool ValidateSchedule(int scheduleId, int userId);
    }
    
    public class ValidationService : IValidationService
    {
        private IUserRepository _userRepository;
        private IFeederRepository _feederRepository;
        private ITagRepository _tagRepository;
        private IScheduleRepository _scheduleRepository;

        public ValidationService(IRepositoryWrapper repository)
        {
            _userRepository = repository.User;
            _feederRepository = repository.Feeder;
            _tagRepository = repository.Tag;
            _scheduleRepository = repository.Schedule;
        }

        public bool ValidateRole(string token, string role)
        {
            var user = _userRepository.FindByCondition(u => u.Remember_Token.Equals(token) &&
                                                            u.Is_Registered == true);
            if (!user.Any())
            {
                return false;
            }
            return user.First().Role.Equals(role);
        }

        public int ValidateUserId(string token)
        {
            var users = _userRepository.FindByCondition(u => u.Remember_Token.Equals(token));
            if (!users.Any())
            {
                return -1;
            }

            return users.First().Id;
        }

        public bool ValidateUserFeeder(int feeder_id, int user_id)
        {
            var feeders = _feederRepository.FindByCondition(f => f.Id == feeder_id && f.User_Id == user_id);
            if (!feeders.Any())
            {
                return false;
            }
            return true;
        }

        public bool ValidateTag(int tagId, int userId)
        {
            var tags = _tagRepository.FindByCondition(t => t.Id == tagId);
            if (!tags.Any()) return false;
            var tag = tags.First();
            var feeders = _feederRepository.FindByCondition(f => f.Id == tag.Feeder_Id);
            if (!feeders.Any()) return false;
            var feeder = feeders.First();
            var users = _userRepository.FindByCondition(u => u.Id == feeder.User_Id);
            if (!users.Any()) return false;
            return true;
        }

        public bool ValidateSchedule(int scheduleId, int userId)
        {
            var schedules = _scheduleRepository.FindByCondition(t => t.Id == scheduleId);
            if (!schedules.Any()) return false;
            var schedule = schedules.First();
            var feeders = _feederRepository.FindByCondition(f => f.Id == schedule.Feeder_Id);
            if (!feeders.Any()) return false;
            var feeder = feeders.First();
            var users = _userRepository.FindByCondition(u => u.Id == feeder.User_Id);
            if (!users.Any()) return false;
            return true;
        }
    }
}