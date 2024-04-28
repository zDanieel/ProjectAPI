using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;


namespace DataAccess
{
    public class RepositoryCustomer<TEntity> : BaseModel<TEntity> where TEntity : Customer, new()
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
