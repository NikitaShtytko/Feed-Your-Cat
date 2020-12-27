using System.Linq;
using FeedYourCat.Helpers;

namespace FeedYourCat.Services
{
    public interface IValidationService
    {
        bool ValidateRole(string token, string role);
        int ValidateUserId(string token);
        bool ValidateUserFeeder(int feeder_id, int user_id);
    }
    
    public class ValidationService : IValidationService
    {
        private IUserRepository _userRepository;
        private IFeederRepository _feederRepository;

        public ValidationService(IRepositoryWrapper repository)
        {
            _userRepository = repository.User;
            _feederRepository = repository.Feeder;
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
            var feeders = _feederRepository.FindByCondition(f => f.Id == feeder_id &&
                                                               f.User_Id == user_id);
            if (!feeders.Any())
            {
                return false;
            }

            return true;
        }
    }
}