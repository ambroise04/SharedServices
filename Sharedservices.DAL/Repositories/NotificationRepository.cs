using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SharedServices.DAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationContext Context;
        public NotificationRepository(ApplicationContext context)
        {
            Context = context;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            var entity = Context.Notifications.Find(id);
            var tracking = Context.Notifications.Remove(entity);

            return tracking.State == EntityState.Deleted;
        }

        public IEnumerable<Notification> GetAll()
        {
            return Context.Notifications
                          .Include(s => s.Type)
                          .Include(s => s.Request)
                          .Include(s => s.User)
                          .Include(s => s.Service)
                          .ToList();
        }

        public Notification GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("A bad id was submitted.");

            return Context.Notifications
                          .Include(s => s.Type)
                          .Include(s => s.Request)
                          .Include(s => s.User)
                          .Include(s => s.Service)
                          .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Notification> GetByPredicate(Expression<Func<Notification, bool>> predicate)
        {
            return Context.Notifications
                          .Include(s => s.Type)
                          .Include(s => s.Request)
                          .Include(s => s.User)
                          .ThenInclude(u => u.Picture)
                          .Include(s => s.RequestMulticast)
                          .ThenInclude(r => r.Service)
                          .Include(s => s.Correspondent)
                          .ThenInclude(c => c.Picture)
                          .Include(s => s.Service)
                          .Where(predicate);
        }

        public Notification Insert(Notification entity)
        {
            if (entity is null)
                throw new ArgumentNullException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");
            entity.Type = Context.NotificationTypes.FirstOrDefault(n => n.Id == entity.Type.Id);
            entity.RequestMulticast = Context.RequestMulticasts.FirstOrDefault(n => n.Id == entity.RequestMulticast.Id);
            var tracking = Context.Notifications.Add(entity);

            return tracking.Entity;
        }

        public Notification Update(Notification entity)
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