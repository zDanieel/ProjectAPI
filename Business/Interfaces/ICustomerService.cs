using Business.Dtos;
using DataAccess.Data;

namespace Business.Interfaces
{
    public interface ICustomerService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        bool CheckIfNameExists(string name);
        TEntity CreateCustomer(CustomerDTO customerDto);
        (TEntity customer, bool changed) UpdateCustomer(CustomerDTO customerDto);
        TEntity GetCustomer(int id);
    }
}
