using SharedServices.DAL.Entities;
using SharedServices.DAL.Repositories;

namespace SharedServices.DAL.Interfaces
{
    public interface INotificationTypeRepository : IRepository<NotificationType>
    {
        void Detach(NotificationType notificationType);
    }
}