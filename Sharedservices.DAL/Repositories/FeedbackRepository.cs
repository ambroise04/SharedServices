using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharedServices.DAL.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ApplicationContext Context;
        public FeedbackRepository(ApplicationContext context)
        {
            Context = context;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            var entity = Context.Feedbacks.Find(id);
            var tracking = Context.Feedbacks.Remove(entity);

            return tracking.State == EntityState.Deleted;
        }

        public IEnumerable<Feedback> GetAll()
        {
            return Context.Feedbacks
                          .Include(f => f.User)
                          .ToList();
        }

        public Feedback GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            return Context.Feedbacks.Find(id);
        }

        public IEnumerable<Feedback> GetByPredicate(Expression<Func<Feedback, bool>> predicate)
        {
            return Context.Feedbacks.Where(predicate);
        }

        public Feedback Insert(Feedback entity)
        {
            if (entity is null)
                throw new ArgumentException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");

            var tracking = Context.Feedbacks.Add(entity);

            return tracking.Entity;
        }

        public Feedback Update(Feedback entity)
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