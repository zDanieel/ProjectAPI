using DataAccess;
using DataAccess.Data;

namespace Business
{
    public class ExtendedWithNameService<TEntity> : BaseService<TEntity> where TEntity : BaseEntityWithNameConstraint, new()
    {
        private readonly BaseModelWithNameConstraint<TEntity> _baseModelWithNameConstraint;

        public ExtendedWithNameService(BaseModelWithNameConstraint<TEntity> baseModelWithNameConstraint) : base(baseModelWithNameConstraint)
        {
            _baseModelWithNameConstraint = baseModelWithNameConstraint;
        }

        public bool CheckIfNameExists(string name)
        {
            return _baseModelWithNameConstraint.CheckIfNameExists(name);
        }
    }
}
