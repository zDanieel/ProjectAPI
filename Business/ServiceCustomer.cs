using DataAccess;
using DataAccess.Data;

namespace Business
{
    public class ServiceCustomer<TEntity> : BaseService<TEntity> where TEntity : Customer, new()
    {
        private readonly RepositoryCustomer<TEntity> _baseModelWithNameConstraint;

        public ServiceCustomer(RepositoryCustomer<TEntity> baseModelWithNameConstraint) : base(baseModelWithNameConstraint)
        {
            _baseModelWithNameConstraint = baseModelWithNameConstraint;
        }

        public bool CheckIfNameExists(string name)
        {
            return _baseModelWithNameConstraint.CheckIfNameExists(name);
        }

        public Customer GetCustomers(int id)
        {
            var customer = _baseModelWithNameConstraint.FindById(id);
            return  _baseModelWithNameConstraint.Include(c => c.Posts);
        }
    }
}
