

using DataAccess.Data;

namespace DataAccess.Interfaces
{
    public interface IRepositoryCustomer<TEntity> : IBaseModel<TEntity> where TEntity : Customer, new()
    {
        bool CheckIfNameExists(string name);
    }
}
