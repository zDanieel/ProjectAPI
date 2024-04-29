using API.Models;
using Business.Dtos;
using Business.Interfaces;
using Business.Utilities;
using Business.Utilities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using CustomerEntity = DataAccess.Data.Customer;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        #region Properties
        private readonly ICustomerManager<CustomerEntity> _customerManager;
        private readonly IPostManager _postManager;
        #endregion

        #region Constructor
        public CustomerController(ICustomerManager<CustomerEntity> customerManager, IPostManager postManager)
        {
            _customerManager = customerManager;
            _postManager = postManager;
        }
        #endregion

        #region Methods
        [HttpGet("customers")]
        public IActionResult GetAll()
        {
            try
            {
                var customers = _customerManager.GetAll();
                if (customers != null)
                {
                    return Ok(new GenericResponseApi<CustomerWebModel>()
                    {
                        ElementsCount = customers.Count(),
                        ErrorCode = "NONE",
                        ErrorMessage = "NONE",
                        Succes = true,
                        MessangeInfo = "Customer results found",
                        Data = customers.Select(x => new CustomerWebModel()
                        {
                            Id = x.CustomerId,
                            Name = x.Name
                        }),

                    });
                }
                else
                {
                    return NotFound(new GenericResponseApi<CustomerWebModel>()
                    {
                        ElementsCount = 0,
                        ErrorCode = "NONE",
                        ErrorMessage = "NONE",
                        Succes = false,
                        MessangeInfo = "No customer results found",
                        Data = null,
                    });
                }

            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorFindCustomers;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while fetching customers: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<CustomerWebModel>()
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
                    return BadRequest(new GenericResponseApi<CustomerWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Invalid input data"
                    });
                }

                var exist = _customerManager.CheckIfNameExists(entityDto.Name);

                if (exist)
                {
                    return Conflict(new GenericResponseApi<CustomerWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "The customer already exists"
                    });
                }

                var createdCustomer = _customerManager.CreateCustomer(entityDto);

                if (createdCustomer == null)
                {
                    return BadRequest(new GenericResponseApi<CustomerWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Error creating customer. Please check your input data."
                    });
                }

                return Ok(new GenericResponseApi<CustomerWebModel>()
                {
                    ElementsCount = 1,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Customer created successfully",
                    Data = new List<CustomerWebModel>
                    {
                        new CustomerWebModel
                        {
                            Id = createdCustomer.CustomerId,
                            Name = createdCustomer.Name
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorCreateCustomer;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while creating a customer: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<CustomerWebModel>()
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
                if (id <= 0 || entityDto == null)
                {
                    return BadRequest(new GenericResponseApi<CustomerWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = id <= 0 ? "Invalid customer ID" : "Invalid input data"
                    });
                }

                var existingCustomer = _customerManager.GetById(id);
                if (existingCustomer == null)
                {
                    return NotFound(new GenericResponseApi<CustomerWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Customer not found"
                    });
                }

                var nameExists = _customerManager.CheckIfNameExists(entityDto.Name);
                if (nameExists || entityDto.Name == existingCustomer.Name)
                {
                    return Conflict(new GenericResponseApi<CustomerWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "The customer name already exists"
                    });
                }

                var (updatedCustomer, changed) = _customerManager.UpdateCustomer(new CustomerEntity
                {
                    CustomerId = existingCustomer.CustomerId,
                    Name = entityDto.Name
                });

                if (!changed)
                {
                    return NotFound(new GenericResponseApi<CustomerWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Customer not found"
                    });
                }

                return Ok(new GenericResponseApi<CustomerWebModel>()
                {
                    ElementsCount = 1,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Customer updated successfully",
                    Data = new List<CustomerWebModel>
                    {
                        new CustomerWebModel
                        {
                            Id = updatedCustomer.CustomerId,
                            Name = updatedCustomer.Name
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorUpdateCustomer;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while updating a customer: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<CustomerWebModel>()
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
                    return BadRequest(new GenericResponseApi<CustomerWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Invalid customer ID"
                    });
                }

                var existingCustomer = _customerManager.GetCustomer(id);

                if (existingCustomer == null)
                {
                    return NotFound(new GenericResponseApi<CustomerWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Customer not found"
                    });
                }

                _postManager.DeleteRange(existingCustomer.Posts);

                var result = _customerManager.Delete(existingCustomer.CustomerId);

                return Ok(new GenericResponseApi<CustomerWebModel>()
                {
                    ElementsCount = 1,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Customer deleted successfully",
                    Data = new List<CustomerWebModel>
                    {
                        new CustomerWebModel
                        {
                            Id = result.CustomerId,
                            Name = result.Name
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorDeleteCustomer;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while deleting a customer: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<CustomerWebModel>()
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



