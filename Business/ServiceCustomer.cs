using DataAccess;
using DataAccess.Data;

namespace Business
{
    public class ServiceCustomer<TEntity> : BaseService<TEntity> where TEntity : Customer, new()
    {
        private readonly RepositoryCustomer<TEntity> _serviceCustomer;

        public ServiceCustomer(RepositoryCustomer<TEntity> serviceCustomer) : base(serviceCustomer)
        {
            _serviceCustomer = serviceCustomer;
        }

        public bool CheckIfNameExists(string name)
        {
            return _serviceCustomer.CheckIfNameExists(name);
        }

        public Customer GetCustomers(int id)
        {
            var customer = _serviceCustomer.FindById(id);
            return  _serviceCustomer.Include(c => c.Posts);
        }
    }
}
