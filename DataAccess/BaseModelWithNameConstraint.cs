using DataAccess.Data;
using System.Linq;


namespace DataAccess
{
    public class BaseModelWithNameConstraint<TEntity> : BaseModel<TEntity> where TEntity : BaseEntityWithNameConstraint, new()
    {
        public BaseModelWithNameConstraint(JujuTestContext context) : base(context)
        {
        }

        public bool CheckIfNameExists(string name)
        {
            return _dbSet.Any(entity => entity.Name == name);
        }
    }


}
