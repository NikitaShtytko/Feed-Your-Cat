﻿using System;
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
    [Authorize]
    [ApiController]
    [Route("")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private ILogService _logService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            ILogService logService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _logService = logService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("/api/auth/sign-in")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                role = user.Role,
                token = tokenString
            });
        }

        [AllowAnonymous]
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
        
        // [AllowAnonymous]
        // [HttpGet("/api/users")]
        // public IActionResult GetAll()
        // {
        //     var users = _userService.GetAll();
        //     var model = _mapper.Map<IList<UserModel>>(users);
        //     return Ok(model);
        // }
        
        [HttpGet("/api/admin/users/moderation/moderated")]
        public IActionResult GetAuth()
        {
            var users = _userService.GetModerated();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }
        
        [AllowAnonymous]
        [HttpGet("/api/email")]
        public bool CheckEmail([FromQuery]string email)
        {
            var user = _userService.GetByEmail(email);
            return user != null && user.Any() ? true : false;
        }
        
        [HttpGet("/api/admin/users/moderation")]
        public IActionResult GetNonModerated()
        {
            var users = _userService.GetNonModerated();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPut("/api/users/{id}")]
        public IActionResult Update(int id, [FromBody]UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("/api/admin/users/approve/{id}")]
        public IActionResult Accept(int id)
        {
            Log log = new Log();
            _userService.Accept(id);
            log.Data = "approve user " + id;
            _logService.Create(log);
            return Ok();
        }
        
        [HttpDelete("/api/admin/users/not-approve/{id}")]
        public IActionResult Delete(int id)
        {
            Log log = new Log();
            _userService.Delete(id);
            log.Data = "delete user " + id;
            _logService.Create(log);
            return Ok();
        }
    }
}