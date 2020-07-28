using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace SharedServices.DAL.Repositories
{
    public class ServiceGroupRepository : IServiceGroupRepository
    {
        private readonly ApplicationContext Context;
        public ServiceGroupRepository(ApplicationContext context)
        {
            Context = context;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            var entity = Context.ServiceGroups.Find(id);
            var tracking = Context.ServiceGroups.Remove(entity);

            return tracking.State == EntityState.Deleted;
        }

        public IEnumerable<ServiceGroup> GetAll()
        {
            return Context.ServiceGroups
                          .Include(sg => sg.Services)
                          .ToList();
        }

        public ServiceGroup GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            return Context.ServiceGroups.Find(id);
        }

        public IEnumerable<ServiceGroup> GetByPredicate(Expression<Func<ServiceGroup, bool>> predicate)
        {
            return Context.ServiceGroups
                          .Where(predicate);
        }

        public ServiceGroup Insert(ServiceGroup entity)
        {
            if (entity is null)
                throw new ArgumentException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");

            var tracking = Context.ServiceGroups.Add(entity);

            return tracking.Entity;
        }

        public ServiceGroup Update(ServiceGroup entity)
        {
            if (entity is null)
                throw new ArgumentException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id <= 0)
                throw new ArgumentException($"Invalid object was submitted. The object has a bad id. {nameof(entity)}");

            Context.Attach(entity).State = EntityState.Modified;

            return entity;
        }
    }
}