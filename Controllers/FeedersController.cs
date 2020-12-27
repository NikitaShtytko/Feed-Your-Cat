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
using FeedYourCat.Models.Feeders;
using FeedYourCat.Models.Users;
using Microsoft.EntityFrameworkCore.Internal;

namespace FeedYourCat.Controllers
{
    [ApiController]
    [Route("")]
    public class FeedersController : ControllerBase
    {
        private IFeederService _feederService;
        private IValidationService _validationService;
        private IMapper _mapper;

        public FeedersController(
            IFeederService feederService,
            IValidationService validationService,
            IMapper mapper)
        {
            _feederService = feederService;
            _validationService = validationService;
            _mapper = mapper;
        }

        [HttpGet("/api/admin/feeders")]
        public IActionResult GetAllFeeders([FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            return Ok(_feederService.GetAllFeeders());
        }

        [HttpGet("/api/admin/moderation/feeders")]
        public IActionResult GetFeederRequests([FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }

            var feeders = _mapper.Map<IList<FeederModel>>(_feederService.GetFeederRequests());
            return Ok(feeders);
        }

        [HttpGet("/api/admin/moderated/feeders")]
        public IActionResult GetRegisteredFeeders([FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }

            var feeders = _mapper.Map<IList<FeederModel>>(_feederService.GetRegisteredFeeders());
            return Ok(feeders);
        }

        [HttpGet("/api/admin/feeders/approve/{id}")]
        public IActionResult Approve(int id, [FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            _feederService.Approve(id);
            return Ok();
        }

        [HttpDelete("/api/admin/feeders/decline/{id}")]
        public IActionResult Delete(int id, [FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }

            _feederService.Decline(id);

            return Ok();
        }

        [HttpGet("/api/user/feeders")]
        public IActionResult GetUserFeeders([FromQuery] string token)
        {
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }

            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }
            var feeders = _mapper.Map<IList<FeederModel>>(_feederService.GetUserFeeders(userId));
            return Ok(feeders);
        }

        [HttpPost("/api/user/feeders/register")]
        public IActionResult RegisterFeeder([FromQuery] string token, [FromBody] NewFeederModel model)
        {
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }

            var feeder = _mapper.Map<Feeder>(model);
            feeder.User_Id = userId;
            _feederService.RegisterFeeder(feeder);
            return Ok();
        }
    }
}