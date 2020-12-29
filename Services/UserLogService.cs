using System;
using System.Collections.Generic;
using FeedYourCat.Entities;

namespace FeedYourCat.Services
{
    public interface IUserLogService
    {
        void AddLog(string log, int user_id);
        IEnumerable<UserLog> GetUserLogs(int id);
    }
    
    public class UserLogService : IUserLogService
    {
        private IUserLogRepository _userLogRepository;

        public UserLogService(IRepositoryWrapper repositoryWrapper)
        {
            _userLogRepository = repositoryWrapper.UserLog;
        }

        public void AddLog(string log, int user_id)
        {
            var logEntity = new UserLog();
            logEntity.Action = log;
            logEntity.User_Id = user_id;
            logEntity.Date = DateTime.Now.ToString();
            _userLogRepository.Create(logEntity);
            _userLogRepository.Save();
        }

        public IEnumerable<UserLog> GetUserLogs(int id)
        {
            return _userLogRepository.FindByCondition(ul => ul.User_Id == id);
        }
    }
}