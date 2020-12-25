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
    }
}