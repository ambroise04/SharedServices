using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharedServices.DAL.Repositories
{
    public class NotificationTypeRepository : INotificationTypeRepository
    {
        private readonly ApplicationContext Context;
        public NotificationTypeRepository(ApplicationContext context)
        {
            Context = context;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            var entity = Context.NotificationTypes.Find(id);
            var tracking = Context.NotificationTypes.Remove(entity);

            return tracking.State == EntityState.Deleted;
        }

        public IEnumerable<NotificationType> GetAll()
        {
            return Context.NotificationTypes.ToList();
        }

        public NotificationType GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            return Context.NotificationTypes.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<NotificationType> GetByPredicate(Expression<Func<NotificationType, bool>> predicate)
        {
            return Context.NotificationTypes.Where(predicate);
        }

        public NotificationType Insert(NotificationType entity)
        {
            if (entity is null)
                throw new ArgumentNullException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");

            var tracking = Context.NotificationTypes.Add(entity);

            return tracking.Entity;
        }

        public NotificationType Update(NotificationType entity)
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