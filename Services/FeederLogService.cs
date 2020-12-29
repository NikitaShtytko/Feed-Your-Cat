using System;
using System.Collections.Generic;
using FeedYourCat.Entities;


namespace FeedYourCat.Services
{
    public interface IFeederLogService
    {
        void AddLog(string log, int feeder_id);
        IEnumerable<FeederLog> GetFeederLogs(int id);
    }
    
    public class FeederLogService : IFeederLogService
    {
        private IFeederLogRepository _feederLogRepository;

        public FeederLogService(IRepositoryWrapper repositoryWrapper)
        {
            _feederLogRepository = repositoryWrapper.FeederLog;
        }
        public void AddLog(string log, int feeder_id)
        {
            var logEntity = new FeederLog();
            logEntity.Action = log;
            logEntity.Date = DateTime.Now.ToString();
            logEntity.Feeder_Id = feeder_id;
            _feederLogRepository.Create(logEntity);
            _feederLogRepository.Save();
        }

        public IEnumerable<FeederLog> GetFeederLogs(int id)
        {
            return _feederLogRepository.FindByCondition(l => l.Feeder_Id == id);
        }
    }
}