using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using FeedYourCat.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using FeedYourCat.Services;
using FeedYourCat.Entities;
using FeedYourCat.Models.Users;
using Microsoft.EntityFrameworkCore.Internal;

namespace FeedYourCat.Controllers
{
    [ApiController]
    [Route("")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IValidationService _validationService;
        private IMapper _mapper;

        public UsersController(
            IUserService userService,
            IValidationService validationService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
            _validationService = validationService;
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
        
        [HttpPost]
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
        public IActionResult GetUserRequests([FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            var users = _mapper.Map<IList<UserModel>>(_userService.GetUserRequests());
            return Ok(users);
        }

        [HttpGet("/api/admin/moderated/users")]
        public IActionResult GetRegisteredUsers([FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            var users = _mapper.Map<IList<UserModel>>(_userService.GetRegisteredUsers());
            return Ok(users);
        }
        
        [HttpGet("/api/admin/users/approve/{id}")]
        public IActionResult Approve(int id, [FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            _userService.Approve(id);
            return Ok();
        }
        
        
        [HttpDelete("/api/admin/users/decline/{id}")]
        public IActionResult Delete(int id, [FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            _userService.Decline(id);
            return Ok();
        }
        
        [HttpGet("/api/admin/users")]
        public IActionResult GetAllUsers([FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            var users = _mapper.Map<IList<UserModel>>(_userService.GetAll());
            return Ok(users);
        }
    }
}