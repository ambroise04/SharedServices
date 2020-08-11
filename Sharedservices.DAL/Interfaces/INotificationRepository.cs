using SharedServices.DAL.Entities;
using SharedServices.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharedServices.DAL.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        IEnumerable<Notification> GetByPredicateWithoutTracking(Expression<Func<Notification, bool>> predicate);
    }
}