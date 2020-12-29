using FeedYourCat.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FeedYourCat.Helpers;

namespace FeedYourCat.Services
{
    public interface IRepositoryBase<T>
    {
        IEnumerable<T> FindAll();
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
    
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DataContext _repositoryContext { get; set; }
        public RepositoryBase(DataContext repositoryContext)
        {
            this._repositoryContext = repositoryContext;
        }
        public IEnumerable<T> FindAll()
        {
            return this._repositoryContext.Set<T>().AsNoTracking();
        }
        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this._repositoryContext.Set<T>().Where(expression).AsNoTracking();
        }
        public void Create(T entity)
        {
            this._repositoryContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            this._repositoryContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            this._repositoryContext.Set<T>().Remove(entity);
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
    
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
    
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DataContext repositoryContext)
            :base(repositoryContext)
        {
        }
    }
    
    public interface IFeederRepository : IRepositoryBase<Feeder>
    {
    }
    
    public class FeederRepository : RepositoryBase<Feeder>, IFeederRepository
    {
        public FeederRepository(DataContext repositoryContext)
            :base(repositoryContext)
        {
        }
    }
    
    public interface ITagRepository : IRepositoryBase<Tag>
    {
    }
    
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(DataContext repositoryContext)
            :base(repositoryContext)
        {
        }
    }
    
    public interface IScheduleRepository : IRepositoryBase<Schedule>
    {
    }
    
    public class ScheduleRepository : RepositoryBase<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(DataContext repositoryContext)
            :base(repositoryContext)
        {
        }
    }
    
    public interface IFeederLogRepository : IRepositoryBase<FeederLog>
    {
        
    }
    
    public class FeederLogRepository : RepositoryBase<FeederLog>, IFeederLogRepository
    {
        public FeederLogRepository(DataContext repositoryContext)
            :base(repositoryContext)
        {
        }
    }
    
    public interface IUserLogRepository : IRepositoryBase<UserLog>
    {
        
    }
    
    public class UserLogRepository : RepositoryBase<UserLog>, IUserLogRepository
    {
        public UserLogRepository(DataContext repositoryContext)
            :base(repositoryContext)
        {
        }
    }
    
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IFeederRepository Feeder { get; }
        ITagRepository Tag { get; }
        IScheduleRepository Schedule { get; }
        IFeederLogRepository FeederLog { get; }
        IUserLogRepository UserLog { get; }
        void Save();
    }
    
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DataContext _repoContext;
        private IUserRepository _user;
        private IFeederRepository _feeder;
        private ITagRepository _tag;
        private IScheduleRepository _schedule;
        private IFeederLogRepository _feederLogRepository;
        private IUserLogRepository _userLogRepository;
        public IUserRepository User {
            get {
                if(_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }
        public IFeederRepository Feeder {
            get {
                if(_feeder == null)
                {
                    _feeder = new FeederRepository(_repoContext);
                }
                return _feeder;
            }
        }
        public ITagRepository Tag {
            get {
                if(_tag == null)
                {
                    _tag = new TagRepository(_repoContext);
                }
                return _tag;
            }
        }

        public IScheduleRepository Schedule
        {
            get {
                if(_schedule == null)
                {
                    _schedule = new ScheduleRepository(_repoContext);
                }
                return _schedule;
            }
        }
        
        public IFeederLogRepository FeederLog
        {
            get {
                if(_feederLogRepository == null)
                {
                    _feederLogRepository = new FeederLogRepository(_repoContext);
                }
                return _feederLogRepository;
            }
        }
        
        public IUserLogRepository UserLog
        {
            get {
                if(_userLogRepository == null)
                {
                    _userLogRepository = new UserLogRepository(_repoContext);
                }
                return _userLogRepository;
            }
        }

        public RepositoryWrapper(DataContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
    
}