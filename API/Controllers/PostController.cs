using Microsoft.AspNetCore.Mvc;
using CustomerEntity = DataAccess.Data.Customer;
using PostEntity = DataAccess.Data.Post;
using Business.Dtos;
using Business.Utilities;
using Business.Utilities.Enum;
using System;
using Serilog;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;
using API.Models;
using Business.Interfaces;
using DataAccess.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        #region Attributes
        private readonly IPostManager _postManager;
        private readonly ICustomerManager<CustomerEntity> _customerManager;
        #endregion

        #region Constructor
        public PostController(IPostManager postManager, ICustomerManager<CustomerEntity> customerManager)
        {
            _postManager = postManager;
            _customerManager = customerManager;
        }
        #endregion

        #region Methods
        [HttpGet("posts")]
        public IActionResult GetAll()
        {
            try
            {
                var posts = _postManager.GetAll();
                if (posts != null)
                {
                    return Ok(new GenericResponseApi<PostWebModel>()
                    {
                        ElementsCount = posts.Count(),
                        ErrorCode = "NONE",
                        ErrorMessage = "NONE",
                        Succes = true,
                        MessangeInfo = "Post results found",
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
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorFindPost;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while fetching posts: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<PostWebModel>()
                {
                    Succes = false,
                    ErrorCode = ((int)ErrorCode.ErrorFindPost).ToString(),
                    ErrorMessage = errorMessage,
                });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] PostDTO entityDto)
        {
            try
            {
                if (entityDto == null)
                {
                    return BadRequest(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Invalid input data"
                    });
                }

                var customer = _customerManager.GetById(entityDto.CustomerId);
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

                var result = _postManager.CreatePost(entityDto);

                if (result == null)
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
            catch (Exception ex)
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

        [HttpPost("create-posts")]
        public IActionResult CreatePosts([FromBody] List<PostDTO> postDTOs)
        {
            try
            {
                if (postDTOs == null || !postDTOs.Any())
                {
                    return BadRequest(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "No posts provided"
                    });
                }

                var createdPosts = new List<PostWebModel>();

                foreach (var postDto in postDTOs)
                {
                    var customer = _customerManager.GetById(postDto.CustomerId);
                    if (customer == null)
                    {
                        return NotFound(new GenericResponseApi<PostWebModel>()
                        {
                            Succes = false,
                            Data = null,
                            ElementsCount = 0,
                            MessangeInfo = $"Customer with ID {postDto.CustomerId} not found"
                        });
                    }

                    var createdPost = _postManager.CreatePost(postDto);

                    createdPosts.Add(new PostWebModel
                    {
                        Id = createdPost.PostId,
                        Title = createdPost.Title,
                        Body = createdPost.Body,
                        Type = createdPost.Type,
                        Category = createdPost.Category
                    });
                }

                return Ok(new GenericResponseApi<PostWebModel>()
                {
                    ElementsCount = createdPosts.Count,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Posts created successfully",
                    Data = createdPosts
                });
            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorCreatingMultiplePosts;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while creating posts: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<PostWebModel>()
                {
                    Succes = false,
                    ErrorCode = ((int)error).ToString(),
                    ErrorMessage = errorMessage,
                });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PostDTO entityDto)
        {
            try
            {
                if (id <= 0 || entityDto == null)
                {
                    return BadRequest(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = id <= 0 ? "Invalid post ID" : "Invalid input data"
                    });
                }

                var existingPost = _postManager.GetById(id);
                if (existingPost == null)
                {
                    return NotFound(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Post not found"
                    });
                }

                var (updatedCustomer, changed) = _postManager.UpdatePost(new PostEntity()
                {
                    Body = entityDto.Body,
                    Category = entityDto.Category,
                    CustomerId = entityDto.CustomerId,
                    PostId = existingPost.PostId,
                    Title = entityDto.Title,
                    Type = entityDto.Type,
                });

                if (!changed)
                {
                    return NotFound(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Post not found"
                    });
                }

                return Ok(new GenericResponseApi<PostWebModel>()
                {
                    ElementsCount = 1,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Post updated successfully",
                    Data = new List<PostWebModel>
                    {
                        new PostWebModel
                        {
                            Body = updatedCustomer.Body,
                            Category = updatedCustomer.Category,
                            CustomerId = updatedCustomer.CustomerId,
                            Id = updatedCustomer.PostId,
                            Title = updatedCustomer.Title,
                            Type = updatedCustomer.Type,
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorUpdateCustomer;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while updating a post: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<PostWebModel>()
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
                    return BadRequest(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Invalid post ID"
                    });
                }

                var existingPost = _postManager.GetById(id);
                if (existingPost == null)
                {
                    return NotFound(new GenericResponseApi<PostWebModel>()
                    {
                        Succes = false,
                        Data = null,
                        ElementsCount = 0,
                        MessangeInfo = "Post not found"
                    });
                }

                var result = _postManager.Delete(existingPost.PostId);

                return Ok(new GenericResponseApi<PostWebModel>()
                {
                    ElementsCount = 1,
                    ErrorCode = "NONE",
                    ErrorMessage = "NONE",
                    Succes = true,
                    MessangeInfo = "Post deleted successfully",
                    Data = new List<PostWebModel>
                    {
                        new PostWebModel
                        {
                            Body = result.Body,
                            Category = result.Category,
                            CustomerId = result.CustomerId,
                            Id = result.PostId,
                            Title = result.Title,
                            Type = result.Type
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                var error = ErrorCode.ErrorDeletePost;
                var errorMessage = ErrorMessages.GetMessage((int)error);
                Log.Error(ex, $"An error occurred while deleting a post: {errorMessage}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseApi<PostWebModel>()
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
