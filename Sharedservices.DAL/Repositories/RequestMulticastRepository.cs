using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SharedServices.DAL.Repositories
{
    public class RequestMulticastRepository : IRequestMulticastRepository
    {
        private readonly ApplicationContext Context;
        public RequestMulticastRepository(ApplicationContext context)
        {
            Context = context;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            var entity = Context.RequestMulticasts.Find(id);
            var tracking = Context.RequestMulticasts.Remove(entity);

            return tracking.State == EntityState.Deleted;
        }

        public IEnumerable<RequestMulticast> GetAll()
        {
            return Context.RequestMulticasts
                          .Include(r => r.Responses)
                          .ThenInclude(resp => resp.Responder)
                          .ThenInclude(resp => resp.Picture)
                          .Include(r => r.Service)
                          .ToList();
        }

        public RequestMulticast GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            return Context.RequestMulticasts
                          .Include(r => r.Responses)
                          .ThenInclude(resp => resp.Responder)
                          .ThenInclude(resp => resp.Picture)
                          .Include(r => r.Service)
                          .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<RequestMulticast> GetByPredicate(Expression<Func<RequestMulticast, bool>> predicate)
        {
            return Context.RequestMulticasts
                          .Include(r => r.Responses)
                          .ThenInclude(resp => resp.Responder)
                          .ThenInclude(resp => resp.Picture)
                          .Include(r => r.Service)
                          .Include(r => r.RequesterMulticast)
                          .ThenInclude(rm => rm.Picture)
                          .Where(predicate);
        }

        public RequestMulticast Insert(RequestMulticast entity)
        {
            if (entity is null)
                throw new ArgumentException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");

            var tracking = Context.RequestMulticasts.Add(entity);

            return tracking.Entity;
        }

        public RequestMulticast Update(RequestMulticast entity)
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