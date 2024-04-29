using AutoMapper;
using Business.Dtos;
using Business.Interfaces;
using DataAccess;
using DataAccess.Data;
using DataAccess.Interfaces;
using System.Linq;

namespace Business
{
    public class CustomerManager : BaseService<Customer>, ICustomerManager<Customer>
    {
        private readonly IRepositoryCustomer<Customer> _repositoryCustomer;
        private readonly IMapper _mapper;

        public CustomerManager(IRepositoryCustomer<Customer> repositoryCustomer, IMapper mapper) : base((BaseModel<Customer>)repositoryCustomer)
        {
            _repositoryCustomer = repositoryCustomer;
            _mapper = mapper;
        }

        public bool CheckIfNameExists(string name)
        {
            return _repositoryCustomer.CheckIfNameExists(name);
        }

        public Customer CreateCustomer(CustomerDTO customerDto)
        {
            var customerEntity = _mapper.Map<Customer>(customerDto);
            return Create(customerEntity);
        }

        public (Customer customer, bool changed) UpdateCustomer(Customer customerEntity)
        {
            return (Update(customerEntity.CustomerId, customerEntity, out bool changed), changed);
        }

        public Customer GetCustomer(int id)
        {
            return _repositoryCustomer.Include(c => c.Posts).FirstOrDefault(c => c.CustomerId == id);
        }

    }

}
