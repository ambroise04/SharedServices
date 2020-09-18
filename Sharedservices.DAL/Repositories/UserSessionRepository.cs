using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.DAL.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly ApplicationContext Context;
        public UserSessionRepository(ApplicationContext context)
        {
            Context = context;
        }

        public IEnumerable<UserSession> GetSessionInfos()
        {
            return Context.UserSessions
                          .OrderByDescending(s => s.SessionDate)
                          .ToList();
        }

        public bool AddSessionInfo(UserSession entity)
        {
            if (entity is null)
                throw new ArgumentNullException($"The object cannot be null. {nameof(entity)}");

            if (entity.Id != 0)
                throw new ArgumentException($"A new object cannot have an id. {nameof(entity)}");

            var tracking = Context.UserSessions.Add(entity);

            return tracking.State == EntityState.Added;
        }

        public bool SessionExists(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException($"Ip address cannot be null or empty. {nameof(ipAddress)}");

            var exists = Context.UserSessions.Any(s => s.Ip.Equals(ipAddress));

            return exists;
        }
    }
}