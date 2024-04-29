
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;

namespace DataAccess.Interfaces
{
    public interface IBaseModel<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll { get; }
        TEntity FindById(object id);
        TEntity Create(TEntity entity);
        TEntity Update(TEntity editedEntity, TEntity originalEntity, out bool changed);
        TEntity Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        void SaveChanges();
        IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes);
    }
}
