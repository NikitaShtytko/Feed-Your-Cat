using System.Linq;
using FeedYourCat.Helpers;

namespace FeedYourCat.Services
{
    public interface IValidationService
    {
        bool ValidateRole(string token, string role);
        int ValidateUserId(string token);
    }
    
    public class ValidationService : IValidationService
    {
        private IUserRepository _userRepository;

        public ValidationService(IRepositoryWrapper repository)
        {
            _userRepository = repository.User;
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
    }
}