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

namespace FeedYourCat.Controllers
{
    [Authorize]
    [ApiController]
    [Route("")]
    public class FeederController : ControllerBase
    {
        private IFeederService _feederService;
        private ILogService _logService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        
        public FeederController(
            IFeederService feederService,
            ILogService logService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _feederService = feederService; 
            _logService = logService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        
        [HttpGet("api/feeders")]
        public IActionResult GetFeeders()
        {
            var feeders = _feederService.GetAll();
            var model = _mapper.Map<IList<FeederModel>>(feeders);
            return Ok(model);
        }
        
        [HttpGet("/api/admin/feeders/moderation")]
        public IActionResult GetNonModerated()
        {
            var feeders = _feederService.GetNonModerated();
            var model = _mapper.Map<IList<FeederModel>>(feeders);
            return Ok(model);
        }
        
        [HttpGet("/api/feeders/{id}")]
        public IActionResult GetById(int id)
        {
            var feeder = _feederService.GetById(id);
            var model = _mapper.Map<FeederModel>(feeder);
            return Ok(model);
        }
        
        [HttpGet("/api/user_feeders/{id}")]
        public IActionResult GetByUserId(int id)
        {
            var feeder = _feederService.GetByUserId(id);
            var model = _mapper.Map<IList<FeederModel>>(feeder);
            return Ok(model);
        }
        
        [AllowAnonymous]
        [HttpPost("/feeder")]
        public IActionResult CreateFeeder([FromBody]NewFeederModel model)
        {
            var feeder = _mapper.Map<Feeder>(model);
            Log log = new Log();
            try
            {
                _feederService.Create(feeder);
                log.Data = "create feeder " + feeder.Id + " by UserID " + feeder.User_Id;
                _logService.Create(log);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("/api/admin/feeders/approve/{id}")]
        public IActionResult Accept(int id)
        {
            Log log = new Log();
            _feederService.Accept(id);
            log.Data = "approve feeder " + id;
            _logService.Create(log);
            return Ok();
        }
        
        [HttpDelete("/api/admin/feeders/not-approve/{id}")]
        public IActionResult Delete(int id)
        {
            Log log = new Log();
            _feederService.Delete(id);
            log.Data = "not approve feeder " + id;
            _logService.Create(log);
            return Ok();
        }
    }
}