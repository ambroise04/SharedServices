using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharedServices.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public TEntity GetById(int id);
        public IEnumerable<TEntity> GetAll();
        public IEnumerable<TEntity> GetByPredicate(Expression<Func<TEntity, bool>> predicate);
        public TEntity Insert(TEntity entity);
        public bool Delete(int id);
        public TEntity Update(TEntity entity);
    }
}