using System.Collections.Generic;
using System.Linq;
using FeedYourCat.Entities;

 namespace FeedYourCat.Services
{
    public interface IFeederService
    {
        IEnumerable<Feeder> GetAllFeeders();
        IEnumerable<Feeder> GetFeederRequests();
        IEnumerable<Feeder> GetRegisteredFeeders();
        IEnumerable<Feeder> GetUserFeeders(int user_id);
        void Approve(int id);
        void Decline(int id);
        void RegisterFeeder(Feeder feeder);
        void UpdateFeeder(Feeder feeder);
        int FillFeeder(int id);
        int FeedCat(int id);
        IEnumerable<Tag> AddTag(Tag tag);
        IEnumerable<Tag> DeleteTag(int id);
        IEnumerable<Tag> GetFeederTags(int id);
        IEnumerable<Schedule> AddFeederSchedule(Schedule schedule);
        IEnumerable<Schedule> DeleteFeederSchedule(int id);
        IEnumerable<Schedule> GetFeederSchedules(int id);
    }
    public class FeederService : IFeederService
    {
        private IFeederRepository _feederRepository;
        private ITagRepository _tagRepository;
        private IScheduleRepository _scheduleRepository;
        private IFeederLogService _feederLogService;
        private IUserLogService _userLogService;

        public FeederService(IRepositoryWrapper repositoryWrapper,
            IFeederLogService feederLogService,
            IUserLogService userLogService)
        {
            _feederRepository = repositoryWrapper.Feeder;
            _tagRepository = repositoryWrapper.Tag;
            _scheduleRepository = repositoryWrapper.Schedule;
            _feederLogService = feederLogService;
            _userLogService = userLogService;
        }

        public IEnumerable<Feeder> GetAllFeeders()
        {
            return _feederRepository.FindAll();
        }

        public IEnumerable<Feeder> GetFeederRequests()
        {
            return _feederRepository.FindByCondition(feeder => feeder.Is_Registered == false);
        }

        public IEnumerable<Feeder> GetRegisteredFeeders()
        {
            return _feederRepository.FindByCondition(feeder => feeder.Is_Registered == true);
        }

        public void Approve(int id)
        {
            var feeders = _feederRepository.FindByCondition(f => f.Id == id &&
                                                                 f.Is_Registered == false);
            if (feeders.Any())
            {
                var feeder = feeders.First();
                feeder.Is_Registered = true;
                _feederRepository.Update(feeder);
                _feederRepository.Save();
            }
        }

        public void Decline(int id)
        {
            var feeders = _feederRepository.FindByCondition(f => f.Id == id &&
                                                                 f.Is_Registered == false);
            if (feeders.Any())
            {
                var feeder = feeders.First();
                _feederRepository.Delete(feeder);
                _feederRepository.Save();
            }
        }

        public IEnumerable<Feeder> GetUserFeeders(int user_id)
        {
            return _feederRepository.FindByCondition(f => f.User_Id == user_id &&
                                                          f.Is_Registered == true);
        }

        public void RegisterFeeder(Feeder feeder)
        {
            if (string.IsNullOrEmpty(feeder.Name) || string.IsNullOrEmpty(feeder.Type))
                return;
            feeder.Is_Registered = false;
            feeder.Fullness = 0;
            feeder.Is_Empty = true;
            _feederRepository.Create(feeder);
            _feederRepository.Save();
            _userLogService.AddLog("Register feeder " + feeder.Name, feeder.User_Id);
        }

        public void UpdateFeeder(Feeder feeder)
        {
            if (string.IsNullOrEmpty(feeder.Name) || string.IsNullOrEmpty(feeder.Type))
                return;
            _feederRepository.Update(feeder);
            _feederRepository.Save();
        }

        public int FillFeeder(int id)
        {
            var feeders = _feederRepository.FindByCondition(f => f.Id == id);
            if (!feeders.Any())
            {
                return -1;
            }
            var feeder = feeders.First();
            feeder.Fullness = 100;
            feeder.Is_Empty = false;
            _feederRepository.Update(feeder);
            _feederRepository.Save();
            _feederLogService.AddLog("Filled", feeder.Id);
            return feeder.Fullness;
        }

        public int FeedCat(int id)
        {
            var feeders = _feederRepository.FindByCondition(f => f.Id == id);
            if (!feeders.Any())
            {
                return -1;
            }
            var feeder = feeders.First();
            feeder.Fullness = feeder.Fullness - 20 >= 0 ? feeder.Fullness - 20 : 0;
            if (feeder.Fullness == 0) feeder.Is_Empty = true;
            _feederRepository.Update(feeder);
            _feederRepository.Save();
            _feederLogService.AddLog("Feed the cat", feeder.Id);
            return feeder.Fullness;
        }

        public IEnumerable<Tag> AddTag(Tag tag)
        {
            _tagRepository.Create(tag);
            _tagRepository.Save();
            _feederLogService.AddLog("Added tag " + tag.Tag_Data, tag.Feeder_Id);
            return _tagRepository.FindByCondition(t => t.Feeder_Id == tag.Feeder_Id);
        }

        public IEnumerable<Tag> DeleteTag(int id)
        {
            var tags = _tagRepository.FindByCondition(t => t.Id == id);
            int feeder_id = -1;
            if (tags.Any())
            {
                var tag = tags.First();
                feeder_id = tag.Feeder_Id;
                _tagRepository.Delete(tag);
                _tagRepository.Save();
                _feederLogService.AddLog("Deleted tag " + tag.Tag_Data, tag.Feeder_Id);
            }
            tags = _tagRepository.FindByCondition(t => t.Feeder_Id == feeder_id);
            return tags;
        }

        public IEnumerable<Tag> GetFeederTags(int id)
        {
            return _tagRepository.FindByCondition(tag => tag.Feeder_Id == id);
        }

        public IEnumerable<Schedule> AddFeederSchedule(Schedule schedule)
        {
            _scheduleRepository.Create(schedule);
            _scheduleRepository.Save();
            _feederLogService.AddLog("Added schedule " + schedule.Time, schedule.Feeder_Id);
            return _scheduleRepository.FindByCondition(s => s.Feeder_Id == schedule.Feeder_Id);
        }

        public IEnumerable<Schedule> DeleteFeederSchedule(int id)
        {
            var schedules = _scheduleRepository.FindByCondition(s => s.Id == id);
            int feeder_id = -1;
            if (schedules.Any())
            {
                var schedule = schedules.First();
                feeder_id = schedule.Feeder_Id;
                _scheduleRepository.Delete(schedule);
                _scheduleRepository.Save();
                _feederLogService.AddLog("Deleted schedule " + schedule.Time, schedule.Feeder_Id);
            }

            return _scheduleRepository.FindByCondition(s => s.Feeder_Id == feeder_id);
        }

        public IEnumerable<Schedule> GetFeederSchedules(int id)
        {
            return _scheduleRepository.FindByCondition(s => s.Feeder_Id == id);
        }
    }
}