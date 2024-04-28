using DataAccess.Data;
using DataAccess.Interfaces;
using System.Linq;


namespace DataAccess
{
    public class RepositoryCustomer<TEntity> : BaseModel<TEntity>, IRepositoryCustomer<TEntity> where TEntity : Customer, new()
    {
        public RepositoryCustomer(JujuTestContext context) : base(context)
        {
        }

        public bool CheckIfNameExists(string name)
        {
            return _dbSet.Any(entity => entity.Name == name);
        }

    }
}
