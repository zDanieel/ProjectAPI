using Business.Interfaces;
using DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        protected IBaseModel<TEntity> _BaseModel;

        public BaseService(IBaseModel<TEntity> baseModel)
        {
            _BaseModel = baseModel;
        }

        #region Repository


        /// <summary>
        /// Consulta todas las entidades
        /// </summary>
        public virtual IQueryable<TEntity> GetAll()
        {
            return _BaseModel.GetAll;
        }

        /// <summary>
        /// Consulta por Id
        /// /// <param name="Id"></param>
        /// </summary>
        public virtual TEntity GetById(int entityId)
        {
            return _BaseModel.FindById(entityId);
        }

        /// <summary>
        /// Crea un entidad (Guarda)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity Create(TEntity entity)
        {
            return _BaseModel.Create(entity);
        }

        /// <summary>
        /// Actualiza la entidad (GUARDA)
        /// </summary>
        /// <param name="editedEntity">Entidad editada</param>
        /// <param name="originalEntity">Entidad Original sin cambios</param>
        /// <param name="changed">Indica si se modifico la entidad</param>
        /// <returns></returns>
        public virtual TEntity Update(object id, TEntity editedEntity, out bool changed)
        {
            TEntity originalEntity = _BaseModel.FindById(id);
            return _BaseModel.Update(editedEntity, originalEntity, out changed);
        }


        /// <summary>
        /// Elimina una entidad (Guarda)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual TEntity Delete(int entityId)
        {
            TEntity originalEntity = _BaseModel.FindById(entityId);
            return _BaseModel.Delete(originalEntity);
        }

        /// <summary>
        /// Elimina una entidad (Guarda)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
           _BaseModel.DeleteRange(entities);   
        }

        /// <summary>
        /// Guardar cambios
        /// </summary>
        public virtual void SaveChanges()
        {
            _BaseModel.SaveChanges();
        }
        #endregion
    }
}
