using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FeedYourCat.Entities;
using FeedYourCat.Helpers;
using FeedYourCat.Models.Feeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
    }
    public class FeederService : IFeederService
    {
        private IFeederRepository _feederRepository;

        public FeederService(IFeederRepository feederRepository)
        {
            _feederRepository = feederRepository;
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
            return _feederRepository.FindByCondition(f => f.User_Id == user_id);
        }

        public void RegisterFeeder(Feeder feeder)
        {
            if (string.IsNullOrEmpty(feeder.Name) || string.IsNullOrEmpty(feeder.Type))
                return;
            feeder.Is_Registered = false;
            _feederRepository.Create(feeder);
            _feederRepository.Save();
        }

        public void UpdateFeeder(Feeder feeder)
        {
            if (string.IsNullOrEmpty(feeder.Name) || string.IsNullOrEmpty(feeder.Type))
                return;
            _feederRepository.Update(feeder);
            _feederRepository.Save();
        }
    }
}