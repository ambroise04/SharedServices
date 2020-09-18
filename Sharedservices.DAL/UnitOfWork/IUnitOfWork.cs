using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Interfaces;

namespace SharedServices.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IDiscussionRepository DiscussionRepository { get; }
        IRequestRepository RequestRepository { get; }
        IRequestMulticastRepository RequestMulticastRepository { get; }
        IServiceGroupRepository ServiceGroupRepository { get; }
        IServiceRepository ServiceRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        IGlobalInfoRepository GlobalInfoRepository { get; }
        INotificationRepository NotificationRepository { get; }
        INotificationTypeRepository NotificationTypeRepository { get; }
        IFaqQuestionRepository FaqQuestionRepository { get; }
        IUserSessionRepository UserSessionRepository { get; }

        void CreateTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void Save();
    }
}