using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharedServices.DAL.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationContext Context;
        public ServiceRepository(ApplicationContext context)
        {
            Context = context;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            var entity = Context.Services.Find(id);
            var tracking = Context.Services.Remove(entity);

            return tracking.State == EntityState.Deleted;
        }

        public IEnumerable<Service> GetAll()
        {
            return Context.Services
                          .Include(s => s.Group)                          
                          .Include(s => s.UserServices)
                          .ThenInclude(us => us.User)
                          .ToList();
        }

        public Service GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            return Context.Services.Include(s => s.Group).FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Service> GetByPredicate(Expression<Func<Service, bool>> predicate)
        {
            return Context.Services
                          .Include(s => s.Group)
                          .Include(s => s.UserServices)
                          .ThenInclude(us => us.User)
                          .Where(predicate);
        }

        public Service Insert(Service entity)
        {
            if (entity is null)
                throw new ArgumentNullException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");

            if (entity.Group is null)
                throw new ArgumentNullException($"The group reference in the service cannot be null. {nameof(entity)}");

            var tracking = Context.Services.Add(entity);

            return tracking.Entity;
        }

        public Service Update(Service entity)
        {
            if (entity is null)
                throw new ArgumentNullException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id <= 0)
                throw new ArgumentException($"Invalid object was submitted. The object has a bad id. {nameof(entity)}");

            Context.Attach(entity).State = EntityState.Modified;

            return entity;
        }
    }
}