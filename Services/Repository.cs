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
    
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IFeederRepository Feeder { get; }
        void Save();
    }
    
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DataContext _repoContext;
        private IUserRepository _user;
        private IFeederRepository _feeder;
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