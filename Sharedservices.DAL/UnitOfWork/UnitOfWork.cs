using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedServices.DAL.Interfaces;
using SharedServices.DAL.Repositories;
using System;

namespace SharedServices.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork<ApplicationContext>, IDisposable
    {
        private readonly ApplicationContext _context;
        private string _errorMessage = string.Empty;
        private IDbContextTransaction _transaction;

        private readonly IDiscussionRepository discussionRepository;
        private readonly IRequestRepository requestRepository;
        private readonly IServiceGroupRepository serviceGroupRepository;
        private readonly IServiceRepository serviceRepository;

        private bool disposedValue = false;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public IDiscussionRepository DiscussionRepository => discussionRepository ?? new DiscussionRepository(_context);

        public IRequestRepository RequestRepository => requestRepository ?? new RequestRepository(_context);

        public IServiceGroupRepository ServiceGroupRepository => serviceGroupRepository ?? new ServiceGroupRepository(_context);

        public IServiceRepository ServiceRepository => serviceRepository ?? new ServiceRepository(_context);

        public void CreateTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
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
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
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