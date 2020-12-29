using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FeedYourCat.Helpers;
using System.Text.Json;
using FeedYourCat.Services;
using FeedYourCat.Entities;
using FeedYourCat.Models.Users;
using Microsoft.AspNetCore.Http;

namespace FeedYourCat.Controllers
{
    [ApiController]
    [Route("")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IValidationService _validationService;
        private IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private IUserLogService _userLogService;

        public UsersController(
            IUserService userService,
            IValidationService validationService,
            IUserLogService userLogService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _userLogService = userLogService;
            _mapper = mapper;
            _validationService = validationService;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpPost("/api/auth/sign-in")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var result = _userService.Authenticate(model.Email, model.Password);

            if (result == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            
            var token = result.Item1;
            var role = result.Item2;
            
            // return basic user info and authentication token
            return Ok(new
            {
                token = token,
                role = role
            });
        }
        
        [HttpPost("/api/auth/sign-up")]
        public IActionResult Register([FromBody]RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);
            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("/api/admin/moderation/users")]
        public IActionResult GetUserRequests()
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            var users = _mapper.Map<IList<UserModel>>(_userService.GetUserRequests());
            return Ok(users);
        }

        [HttpGet("/api/admin/moderated/users")]
        public IActionResult GetRegisteredUsers()
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            var users = _mapper.Map<IList<UserListModel>>(_userService.GetRegisteredUsers());
            return Ok(users);
        }
        
        [HttpGet("/api/admin/users/approve/{id}")]
        public IActionResult Approve(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            _userService.Approve(id);
            return Ok();
        }
        
        
        [HttpDelete("/api/admin/users/decline/{id}")]
        public IActionResult Delete(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            _userService.Decline(id);
            return Ok();
        }
        
        [HttpGet("/api/admin/users")]
        public IActionResult GetAllUsers()
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            var users = _mapper.Map<IList<UserModel>>(_userService.GetAll());
            return Ok(users);
        }
        
        [HttpGet("/api/admin/users/log/{id}")]
        public IActionResult GetUserLogsAdmin(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }

            var logs = _userLogService.GetUserLogs(id);
            string data = "";
            foreach(var item in logs)
            {
                data += item.Action + " " + item.Date + "\n";
            }

            var obj = JsonSerializer.Serialize(data);
            return Ok(obj);
        }
    }
}