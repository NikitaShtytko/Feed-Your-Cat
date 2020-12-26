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
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        
        public FeederController(
            IFeederService feederService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _feederService = feederService;
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
        
        [HttpPost("/feeder")]
        public IActionResult CreateFeeder([FromBody]NewFeederModel model)
        {
            var feeder = _mapper.Map<Feeder>(model);
            try
            {
                _feederService.Create(feeder);
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
            _feederService.Accept(id);
            return Ok();
        }
        
        [HttpDelete("/api/admin/feeders/not-approve/{id}")]
        public IActionResult Delete(int id)
        {
            _feederService.Delete(id);
            return Ok();
        }
    }
}