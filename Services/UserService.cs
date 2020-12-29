using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FeedYourCat.Entities;
using FeedYourCat.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FeedYourCat.Services
{
    public interface IUserService
    {
        Tuple<string ,string> Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        public IEnumerable<User> GetRegisteredUsers();
        public IEnumerable<User> GetUserRequests();
        void Create(User user, string password);
        void Approve(int id);
        void Decline(int id);
    }

    public class UserService : IUserService
    {
        
        private IUserRepository _userRepository;
        private IUserLogService _userLogService;
        private readonly AppSettings _appSettings;

        public UserService(IUserRepository userRepository,
            IUserLogService userLogService,
            IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _userLogService = userLogService;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.FindAll();
        }

        public Tuple<string ,string> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var users = _userRepository.FindByCondition(u => u.Email.Equals(email) 
                                                             && u.Is_Registered == true);
            if(!users.Any())
            {
                return null;
            }

            var user = users.First();
            
            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
        
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,  user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            user.Remember_Token = tokenString;
            _userRepository.Update(user);
            _userRepository.Save();
            
            Tuple<string, string> result = new Tuple<string, string>(tokenString, user.Role);
            
            // authentication successful
            return result;
        }
        
        public IEnumerable<User> GetRegisteredUsers()
        {
            return _userRepository.FindByCondition(user => user.Is_Registered == true);
        }

        public IEnumerable<User> GetUserRequests()
        {
            return _userRepository.FindByCondition(user => user.Is_Registered == false);
        }
        
        public void Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");
        
            if (_userRepository.FindByCondition(x => x.Email.Equals(user.Email)).Any())
                throw new AppException("Email \"" + user.Email + "\" is already taken");
        
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
        
            _userRepository.Create(user);
            _userRepository.Save();
        }
        
        public void Approve(int id)
        {
            var users = _userRepository.FindByCondition(u => u.Id == id && 
                                                             u.Is_Registered == false);
            if (users.Any())
            {
                var user = users.First();
                user.Is_Registered = true;
                _userRepository.Update(user);
                _userRepository.Save();
                _userLogService.AddLog("Approved", user.Id);
            }
        }
        
        public void Decline(int id)
        {
            var users = _userRepository.FindByCondition(u => u.Id == id && 
                                                             u.Is_Registered == false);
            if (users.Any())
            {
                var user = users.First();
                _userRepository.Delete(user);
                _userRepository.Save();
                _userLogService.AddLog("Declined", user.Id);
            }
        }
        
        
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
        
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }
        
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
        
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
        
            return true;
        }
    }
}