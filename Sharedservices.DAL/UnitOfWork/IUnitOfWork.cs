using Microsoft.EntityFrameworkCore;
using SharedServices.DAL.Interfaces;

namespace SharedServices.DAL.UnitOfWork
{
    public interface IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        IDiscussionRepository DiscussionRepository { get; }
        IRequestRepository RequestRepository { get; }
        IServiceGroupRepository ServiceGroupRepository { get; }
        IServiceRepository ServiceRepository { get; }

        void CreateTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void Save();
    }
}