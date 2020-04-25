using System.Collections.Generic;

namespace SharedServices.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public TEntity GetById(int id);
        public IEnumerable<TEntity> GetAll();
        public TEntity Insert(TEntity entity);
        public bool Delete(int id);
        public TEntity Update(TEntity entity);
    }
}