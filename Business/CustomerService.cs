using AutoMapper;
using Business.Dtos;
using Business.Interfaces;
using DataAccess;
using DataAccess.Data;
using DataAccess.Interfaces;

namespace Business
{
    public class CustomerService : BaseService<Customer>, ICustomerService<Customer>
    {
        private readonly IRepositoryCustomer<Customer> _repositoryCustomer;
        private readonly IMapper _mapper;

        public CustomerService(IRepositoryCustomer<Customer> serviceCustomer, IMapper mapper) : base((BaseModel<Customer>)serviceCustomer)
        {
            _repositoryCustomer = serviceCustomer;
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

        public (Customer customer, bool changed) UpdateCustomer(CustomerDTO customerDto)
        {
            var customerEntity = _mapper.Map<Customer>(customerDto);
            return (Update(customerEntity.CustomerId, customerEntity, out bool changed), changed);
        }

        public Customer GetCustomer(int id)
        {
            var customer = _repositoryCustomer.FindById(id);
            return customer;
        }
    }

}
