using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FeedYourCat.Services;
using FeedYourCat.Entities;
using FeedYourCat.Models.Feeders;
using Microsoft.AspNetCore.Http;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FeedYourCat.Controllers
{
    [ApiController]
    [Route("")]
    public class FeedersController : ControllerBase
    {
        private IFeederService _feederService;
        private IValidationService _validationService;
        private IHttpContextAccessor _httpContextAccessor;
        private IFeederLogService _feederLogService;
        private IMapper _mapper;

        public FeedersController(
            IHttpContextAccessor httpContextAccessor,
            IFeederService feederService,
            IFeederLogService feederLogService,
            IValidationService validationService,
            IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _feederService = feederService;
            _validationService = validationService;
            _mapper = mapper;
            _feederLogService = feederLogService;
        }

        [HttpGet("/api/admin/feeders")]
        public IActionResult GetAllFeeders()
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            return Ok(_feederService.GetAllFeeders());
        }

        [HttpGet("/api/admin/moderation/feeders")]
        public IActionResult GetFeederRequests()
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }

            var feeders = _mapper.Map<IList<FeederModel>>(_feederService.GetFeederRequests());
            return Ok(feeders);
        }

        [HttpGet("/api/admin/moderated/feeders")]
        public IActionResult GetRegisteredFeeders()
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }

            var feeders = _mapper.Map<IList<FeederModel>>(_feederService.GetRegisteredFeeders());
            return Ok(feeders);
        }

        [HttpGet("/api/admin/feeders/approve/{id}")]
        public IActionResult Approve(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }
            _feederService.Approve(id);
            return Ok();
        }

        [HttpDelete("/api/admin/feeders/decline/{id}")]
        public IActionResult Delete(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }

            _feederService.Decline(id);

            return Ok();
        }

        [HttpGet("/api/user/feeders")]
        public IActionResult GetUserFeeders()
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
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
        public IActionResult RegisterFeeder([FromBody] NewFeederModel model)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
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

        [HttpPost("/api/user/feeders/redact")]
        public IActionResult RedactFeeder([FromBody] FeederModel model)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }

            if (!_validationService.ValidateUserFeeder(model.User_Id, userId))
            {
                return BadRequest("It's not your feeder!");
            }

            var feeder = _mapper.Map<Feeder>(model);
            _feederService.UpdateFeeder(feeder);
            return Ok();
        }

        [HttpGet("/api/user/feeders/fill/{id}")]
        public IActionResult FillFeeder(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }

            if (!_validationService.ValidateUserFeeder(id, userId))
            {
                return BadRequest("It's not your feeder!");
            }
            int newFullness = _feederService.FillFeeder(id);
            return Ok(newFullness);
        }

        [HttpGet("/api/user/feeders/feed/{id}")]
        public IActionResult FeedCat(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }
            if (!_validationService.ValidateUserFeeder(id, userId))
            {
                return BadRequest("It's not your feeder!");
            }

            int newFullness = _feederService.FeedCat(id);
            return Ok(newFullness);
        }

        [HttpPut("/api/user/feeders/tag")]
        public IActionResult AddFeederTag([FromBody] TagModel model)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }
            if (!_validationService.ValidateUserFeeder(model.Id, userId))
            {
                return BadRequest("It's not your feeder!");
            }

            var tag = _mapper.Map<Tag>(model);
            tag.Feeder_Id = tag.Id;
            tag.Id = 0;

            return Ok(_feederService.AddTag(tag));
        }

        [HttpDelete("/api/user/feeders/tag/{id}")]
        public IActionResult DeleteTag(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }
            if (!_validationService.ValidateTag(id, userId))
            {
                return BadRequest("Tag validation failed");
            }
            var tags = _feederService.DeleteTag(id);
            return Ok(tags);
        }

        [HttpGet("/api/user/feeders/tags/{id}")]
        public IActionResult GetUserTags(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }
            if (!_validationService.ValidateUserFeeder(id, userId))
            {
                return BadRequest("It's not your feeder!");
            }

            var tags = _feederService.GetFeederTags(id);
            return Ok(tags);
        }

        [HttpPut("/api/user/feeders/schedule")]
        public IActionResult AddFeederSchedule([FromBody] ScheduleModel model)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }
            if (!_validationService.ValidateUserFeeder(model.Id, userId))
            {
                return BadRequest("It's not your feeder!");
            }
            
            var schedule = _mapper.Map<Schedule>(model);
            schedule.Feeder_Id = schedule.Id;
            schedule.Id = 0;
            var schedules = _feederService.AddFeederSchedule(schedule);
            return Ok(schedules);
        }

        [HttpDelete("/api/user/feeders/schedule/{id}")]
        public IActionResult DeleteFeederSchedule(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }
            if (!_validationService.ValidateSchedule(id, userId))
            {
                return BadRequest("Schedule validation failed");
            }

            var schedules = _feederService.DeleteFeederSchedule(id);
            return Ok(schedules);
        }

        [HttpGet("/api/user/feeders/schedules/{id}")]
        public IActionResult GetFeederSchedules(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }
            if (!_validationService.ValidateUserFeeder(id, userId))
            {
                return BadRequest("It's not your feeder!");
            }

            var schedules = _feederService.GetFeederSchedules(id);
            return Ok(schedules);
        }

        [HttpGet("/api/user/feeders/log/{id}")]
        public IActionResult GetFeederLogs(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            
            if (!_validationService.ValidateRole(token, "user"))
            {
                return BadRequest("You are not user!");
            }
            int userId = _validationService.ValidateUserId(token);
            if (userId == -1)
            {
                return BadRequest("You are not user!");
            }
            if (!_validationService.ValidateUserFeeder(id, userId))
            {
                return BadRequest("It's not your feeder!");
            }
            
            var logs = _feederLogService.GetFeederLogs(id);
            string data = "";
            foreach(var item in logs)
            {
                data += item.Action + " " + item.Date + "\n";
            }

            var obj = JsonSerializer.Serialize(data);
            return Ok(obj);
        }
        
        [HttpGet("/api/admin/feeders/log/{id}")]
        public IActionResult GetFeederLogsAdmin(int id)
        {
            string tokenBase = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var token = tokenBase.Split(" ")[1];
            
            if (!_validationService.ValidateRole(token, "admin"))
            {
                return BadRequest("You are not admin!");
            }

            var logs = _feederLogService.GetFeederLogs(id);
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