using System.Collections.Generic;
using System.Linq;

namespace Business.Interfaces
{
    public interface IBaseService<TEntity>
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(int entityId);
        TEntity Create(TEntity entity);
        TEntity Update(object id, TEntity editedEntity, out bool changed);
        TEntity Delete(int entityId);
        void DeleteRange(IEnumerable<TEntity> entities);
        void SaveChanges();
    }
}
