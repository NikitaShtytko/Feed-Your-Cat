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
            return _context.Feeders.Where(feeder => feeder.User_Id == id);
        }
    }
}