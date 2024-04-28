using API.Models.Customer;
using AutoMapper;
using Business;
using Business.Dtos;
using Business.Utilities;
using Business.Utilities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using CustomerEntity = DataAccess.Data.Customer;
using PostEntity = DataAccess.Data.Post;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        #region Properties
        private readonly ServiceCustomer<CustomerEntity> _customerService;
        private readonly BaseService<PostEntity> _postService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public CustomerController(ServiceCustomer<CustomerEntity> customerService, BaseService<PostEntity> postService, IMapper mapper)
        {
            _customerService = customerService;
            _postService = postService;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet("customers")]
        public IActionResult GetAll()
        {
            try
            {
                var customers = _customerService.GetAll();
                if (customers != null)
                {
                    return Ok(new CustomerWebModelResponse()
                    {
                        ElementsCount = customers.Count(),
                        ErrorCode = "NONE",
                        ErrorMessage = "NONE",
                        Succes = true,
                        MessangeInfo = "Customer results found", 
                        Customers = customers.Select(x => new CustomerWebModel() { Id = x.CustomerId, Name = x.Name }),

                    });
                }
                else
                {
                    return NotFound(new CustomerWebModelResponse()
                    {
                        ElementsCount = 0,
                        ErrorCode = "NONE",
                        ErrorMessage = "NONE",
                        Succes = false,
                        MessangeInfo = "No customer results found",
                        Customers = null,
                    });
                }

        }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorFindCustomers;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while fetching customers: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomerWebModelResponse()
                {
                    Succes = false,
                    ErrorCode = ((int)ErrorCode.ErrorFindCustomers).ToString(),
                    ErrorMessage = errorMessage,
                });
            }

        }

        [HttpPost]
        public IActionResult Create([FromBody] CustomerDTO entityDto)
        {
            try
            {
                if (entityDto == null)
                {
                    return BadRequest(new CustomerWebModelResponse
                    {
                        Succes = false,
                        Customers = null,
                        ElementsCount = 0,
                        MessangeInfo = "Invalid input data"
                    });
                }

                var exist = _customerService.CheckIfNameExists(entityDto.Name);

                if (exist)
                {
                    return Conflict(new CustomerWebModelResponse
                    {
                        Succes = false,
                        Customers = null,
                        ElementsCount = 0,
                        MessangeInfo = "The customer already exists"
                    });
                }

                var customerEntity = _mapper.Map<CustomerEntity>(entityDto);

                var createdCustomer = _customerService.Create(customerEntity);

                return Ok(new CustomerWebModelResponse
                {
                    ElementsCount = 1,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Customer created successfully",
                    Customers = new List<CustomerWebModel> { new CustomerWebModel { Id = createdCustomer.CustomerId, Name = createdCustomer.Name } }
                });
            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorCreateCustomer;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while creating a customer: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomerWebModelResponse
                {
                    Succes = false,
                    ErrorCode = ((int)error).ToString(),
                    ErrorMessage = errorMessage,
                });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CustomerDTO entityDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new CustomerWebModelResponse
                    {
                        Succes = false,
                        Customers = null,
                        ElementsCount = 0,
                        MessangeInfo = "Invalid customer ID"
                    });
                }

                if (entityDto == null)
                {
                    return BadRequest(new CustomerWebModelResponse
                    {
                        Succes = false,
                        Customers = null,
                        ElementsCount = 0,
                        MessangeInfo = "Invalid input data"
                    });
                }


                var existingCustomer = _customerService.GetById(id);
                if (existingCustomer == null)
                {
                    return NotFound(new CustomerWebModelResponse
                    {
                        Succes = false,
                        Customers = null,
                        ElementsCount = 0,
                        MessangeInfo = "Customer not found"
                    });
                }

                var nameExists = _customerService.CheckIfNameExists(entityDto.Name);
                if (nameExists || entityDto.Name == existingCustomer.Name)
                {
                    return Conflict(new CustomerWebModelResponse
                    {
                        Succes = false,
                        Customers = null,
                        ElementsCount = 0,
                        MessangeInfo = "The customer name already exists"
                    });
                }

                var customerEntity = _mapper.Map(entityDto, existingCustomer);

                var result = _customerService.Update(customerEntity.CustomerId, customerEntity, out bool changed);

                return Ok(new CustomerWebModelResponse
                {
                    ElementsCount = 1,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Customer updated successfully",
                    Customers = new List<CustomerWebModel> { new CustomerWebModel { Id = result.CustomerId, Name = result.Name } }
                });
            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorUpdateCustomer;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while updating a customer: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomerWebModelResponse
                {
                    Succes = false,
                    ErrorCode = ((int)error).ToString(),
                    ErrorMessage = errorMessage,
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new CustomerWebModelResponse
                    {
                        Succes = false,
                        Customers = null,
                        ElementsCount = 0,
                        MessangeInfo = "c customer ID"
                    });
                }

                var existingCustomer = _customerService.GetCustomers(id);
                if (existingCustomer == null)
                {
                    return NotFound(new CustomerWebModelResponse
                    {
                        Succes = false,
                        Customers = null,
                        ElementsCount = 0,
                        MessangeInfo = "Customer not found"
                    });
                }

                _postService.DeleteRange(existingCustomer.Posts);

                var result = _customerService.Delete(existingCustomer.CustomerId);

                return Ok(new CustomerWebModelResponse
                {
                    ElementsCount = 1,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Customer deleted successfully",
                    Customers = new List<CustomerWebModel> { new CustomerWebModel { Id = result.CustomerId, Name = result.Name } }
                });
            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorDeleteCustomer;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while deleting a customer: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CustomerWebModelResponse
                {
                    Succes = false,
                    ErrorCode = ((int)error).ToString(),
                    ErrorMessage = errorMessage,
                });
            }
        }
        #endregion
    }
}



