using System;
using System.Collections.Generic;
using System.Linq;
using FeedYourCat.Entities;
using FeedYourCat.Helpers;
using Microsoft.AspNetCore.Identity;

namespace FeedYourCat.Services
{
    public interface IFeederService
    {
        IEnumerable<Feeder> GetAll();
        Feeder GetById(int id);
        IEnumerable<Feeder> GetByUserId(int id);
        Feeder Create(Feeder feeder);
        IEnumerable<Feeder> GetNonModerated();
        void Accept(int id);
        void Delete(int id);
    }
    public class FeederService : IFeederService
    {
        private DataContext _context;

        public FeederService(DataContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Feeder> GetAll()
        {
            return _context.Feeders;
        }

        public Feeder GetById(int id)
        {
            return _context.Feeders.Find(id);
        }

        public IEnumerable<Feeder> GetByUserId(int id)
        {
            return _context.Feeders.Where(feeder => feeder.User_Id == id && feeder.Status == true);
        }

        public Feeder Create(Feeder feeder)
        {
            feeder.Status = false;
            _context.Feeders.Add(feeder);
            _context.SaveChanges();
            return feeder;
        }

        public IEnumerable<Feeder> GetNonModerated()
        {
            return _context.Feeders.Where(feeder => feeder.Status == false);
        }

        public void Accept(int id)
        {
            var feeder = _context.Feeders.Find(id);
            if (feeder != null)
            {
                feeder.Status = true;
                _context.Feeders.Update(feeder);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var feeder = _context.Feeders.Find(id);
            if (feeder != null)
            {
                _context.Feeders.Remove(feeder);
                _context.SaveChanges();
            }
        }
    }
}