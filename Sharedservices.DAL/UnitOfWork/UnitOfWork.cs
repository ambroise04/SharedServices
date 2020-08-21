using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedServices.DAL.Interfaces;
using SharedServices.DAL.Repositories;
using System;

namespace SharedServices.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationContext _context;
        private string _errorMessage = string.Empty;
        private IDbContextTransaction _transaction;

        private readonly IDiscussionRepository discussionRepository;
        private readonly IRequestRepository requestRepository;
        private readonly IRequestMulticastRepository requestMulticastRepository;
        private readonly IServiceGroupRepository serviceGroupRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly IFeedbackRepository feedbackRepository;
        private readonly IGlobalInfoRepository globalInfoRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly INotificationTypeRepository notificationTypeRepository;
        private readonly IFaqQuestionRepository faqQuestionRepository;

        private bool disposedValue = false;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public IDiscussionRepository DiscussionRepository => discussionRepository ?? new DiscussionRepository(_context);

        public IRequestRepository RequestRepository => requestRepository ?? new RequestRepository(_context);
        public IRequestMulticastRepository RequestMulticastRepository => requestMulticastRepository ?? new RequestMulticastRepository(_context);
        public IServiceGroupRepository ServiceGroupRepository => serviceGroupRepository ?? new ServiceGroupRepository(_context);
        public IServiceRepository ServiceRepository => serviceRepository ?? new ServiceRepository(_context);
        public IFeedbackRepository FeedbackRepository => feedbackRepository ?? new FeedbackRepository(_context);
        public IGlobalInfoRepository GlobalInfoRepository => globalInfoRepository ?? new GlobalInfoRepository(_context);
        public INotificationRepository NotificationRepository => notificationRepository ?? new NotificationRepository(_context);
        public INotificationTypeRepository NotificationTypeRepository => notificationTypeRepository ?? new NotificationTypeRepository(_context);
        public IFaqQuestionRepository FaqQuestionRepository => faqQuestionRepository ?? new FaqQuestionRepository(_context);

        public void CreateTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
            Save();
        }

        public void RollbackTransaction()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();
                Dispose();
            }
            catch (DbUpdateException ex)
            {
                _errorMessage = $"Message : {ex.Message}";
                throw new Exception(_errorMessage, ex);
            }
            catch(Exception)
            {
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //_context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}