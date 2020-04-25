using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SharedServices.DAL.Repositories
{
    public class DiscussionRepository : IDiscussionRepository, IRepository<Discussion>
    {
        private readonly ApplicationContext Context;
        public DiscussionRepository(ApplicationContext context)
        {
            Context = context;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            var entity = Context.Discussions.Find(id);
            var tracking = Context.Discussions.Remove(entity);

            return tracking.State == EntityState.Deleted;
        }

        public IEnumerable<Discussion> GetAll()
        {
            return Context.Discussions.ToList();
        }

        public Discussion GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            return Context.Discussions.Find(id);
        }

        public Discussion Insert(Discussion entity)
        {
            if (entity is null)
                throw new ArgumentException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");

            var tracking = Context.Discussions.Add(entity);

            return tracking.Entity;
        }

        public Discussion Update(Discussion entity)
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