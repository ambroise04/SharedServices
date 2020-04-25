using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SharedServices.DAL.Repositories
{
    public class RequestRepository : IRequestRepository, IRepository<Request>
    {
        private readonly ApplicationContext Context;
        public RequestRepository(ApplicationContext context)
        {
            Context = context;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            var entity = Context.Requests.Find(id);
            var tracking = Context.Requests.Remove(entity);

            return tracking.State == EntityState.Deleted;
        }

        public IEnumerable<Request> GetAll()
        {
            return Context.Requests
                          .Include(r => r.User)
                          .Include(r => r.Service)
                          .ToList();
        }

        public Request GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            return Context.Requests.Find(id);
        }

        public Request Insert(Request entity)
        {
            if (entity is null)
                throw new ArgumentException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");

            var tracking = Context.Requests.Add(entity);

            return tracking.Entity;
        }

        public Request Update(Request entity)
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