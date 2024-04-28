using Business;
using Microsoft.AspNetCore.Mvc;
using PostEntity = DataAccess.Data.Post;
using CustomerEntity = DataAccess.Data.Customer;
using Business.Dtos;
using Business.Utilities;
using Business.Utilities.Enum;
using AutoMapper;
using System;
using Serilog;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using API.Models;
using Business.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        #region Attributes
        private readonly IPostService _postService;
        private readonly ICustomerService<CustomerEntity> _customerService;
        #endregion

        #region Constructor
        public PostController(IPostService postService, ICustomerService<CustomerEntity> customerService)
        {
            _postService = postService;
            _customerService = customerService;
        }
        #endregion

        #region Methods
        [HttpGet("posts")]
        public IActionResult GetAll()
        {
            try
            {
                var posts = _postService.GetAll();
                if(posts != null)
                {
                    return Ok(new GenericResponseApi<PostWebModel>()
                    {
                        ElementsCount = posts.Count(),
                        ErrorCode = "NONE",
                        ErrorMessage = "NONE",
                        Succes = true,
                        MessangeInfo = "Customer results found",
                        Data = posts.Select(x => new PostWebModel() 
                        { 
                            Body = x.Body,  
                            Category = x.Category, 
                            CustomerId = x.CustomerId, 
                            Id = x.PostId, 
                            Title = x.Title, 
                            Type = x.Type 
                        })
                    });
                }
                else
                {
                    return NotFound(new GenericResponseApi<PostWebModel>()
                    {
                        ElementsCount = 0,
                        ErrorCode = "NONE",
                        ErrorMessage = "NONE",
                        Succes = false,
                        MessangeInfo = "No posts results found",
                        Data = null,
                    });
                }
            }
            catch(Exception ex)
            {
                var error = ErrorCode.ErrorFindPost;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while fetching posts: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<PostWebModel>()
                {
                    Succes = false,
                    ErrorCode = ((int)ErrorCode.ErrorFindCustomers).ToString(),
                    ErrorMessage = errorMessage,
                });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] PostDTO entityDto)
        {
            try
            {
                if(entityDto == null)
                {
                    return BadRequest(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Invalid input data"
                    });
                }

                var customer = _customerService.GetById(entityDto.CustomerId);
                if (customer == null)
                {
                    return NotFound(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Customer does not exist"
                    });
                }

                var result = _postService.CreatePost(entityDto);

                if(result == null)
                {
                    return BadRequest(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Error creating post. Please check your input data."
                    });
                }

                return Ok(new GenericResponseApi<PostWebModel>()
                {
                    ElementsCount = 1,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Post created successfully",
                    Data = new List<PostWebModel>
                    {
                        new PostWebModel
                        {
                            Body = result.Body,
                            Category = result.Category,
                            CustomerId = result.CustomerId,
                            Id = result.PostId,
                            Title = result.Title,
                            Type = result.Type,
                        }
                    }
                });               
            }
            catch(Exception ex)
            {
                var error = ErrorCode.ErrorCreatePost;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while creating a post: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<PostWebModel>()
                {
                    Succes = false,
                    ErrorCode = ((int)error).ToString(),
                    ErrorMessage = errorMessage,
                });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] PostEntity entity)
        {
            return Ok(_postService.Create(entity));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromBody] PostEntity entity)
        {
            return Ok(_postService.Create(entity));
        }
        #endregion
    }
}
