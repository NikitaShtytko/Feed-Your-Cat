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
        
        [AllowAnonymous]
        [HttpGet("api/feeders")]
        public IActionResult GetFeeders()
        {
            var feeders = _feederService.GetAll();
            var model = _mapper.Map<IList<FeederModel>>(feeders);
            return Ok(model);
        }
        
        [AllowAnonymous]
        [HttpGet("/api/feeders/{id}")]
        public IActionResult GetById(int id)
        {
            var feeder = _feederService.GetById(id);
            var model = _mapper.Map<FeederModel>(feeder);
            return Ok(model);
        }
        
        [AllowAnonymous]
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
    }
}