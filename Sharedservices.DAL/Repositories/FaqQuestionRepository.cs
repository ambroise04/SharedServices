using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharedServices.DAL.Repositories
{
    public class FaqQuestionRepository : IFaqQuestionRepository
    {
        private readonly ApplicationContext Context;
        public FaqQuestionRepository(ApplicationContext context)
        {
            Context = context;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            var entity = Context.FaqQuestions.Find(id);
            var tracking = Context.FaqQuestions.Remove(entity);

            return tracking.State == EntityState.Deleted;
        }

        public IEnumerable<FaqQuestion> GetAll()
        {
            return Context.FaqQuestions
                          .Include(q => q.User)
                          .ThenInclude(u => u.Picture)
                          .Include(q => q.Responses)
                          .ToList();
        }

        public FaqQuestion GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            return Context.FaqQuestions                          
                          .Include(q => q.Responses)
                          .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<FaqQuestion> GetByPredicate(Expression<Func<FaqQuestion, bool>> predicate)
        {
            return Context.FaqQuestions
                          .Include(q => q.User)
                          .ThenInclude(u => u.Picture)
                          .Include(q => q.Responses)
                          .Where(predicate);
        }

        public FaqQuestion Insert(FaqQuestion entity)
        {
            if (entity is null)
                throw new ArgumentNullException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");

            var tracking = Context.FaqQuestions.Add(entity);

            return tracking.Entity;
        }

        public FaqQuestion Update(FaqQuestion entity)
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