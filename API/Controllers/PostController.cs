using Business;
using Microsoft.AspNetCore.Mvc;
using PostEntity = DataAccess.Data.Post;
using CustomerEntity = DataAccess.Data.Customer;
using Business.Dtos;
using API.Models.Post;
using Business.Utilities;
using Business.Utilities.Enum;
using AutoMapper;
using System;
using API.Models.Customer;
using Serilog;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly BaseService<PostEntity> _postService;
        private readonly BaseService<CustomerEntity> _customerService;
        private readonly IMapper _mapper;

        public PostController(BaseService<PostEntity> postService, BaseService<CustomerEntity> customerService, IMapper mapper)
        {
            _postService = postService;
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet("posts")]
        public IActionResult GetAll()
        {
            return Ok(_postService.GetAll());
        }

        [HttpPost()]
        public IActionResult Create([FromBody] PostDTO entityDto)
        {
            try
            {
                var customer = _customerService.GetById(entityDto.CustomerId);
                if (customer == null)
                {
                    return NotFound(new PostWebModelResponse());
                }

                if (!string.IsNullOrEmpty(entityDto.Body) && entityDto.Body.Length > 20)
                {
                    entityDto.Body = entityDto.Body.Substring(0, 97) + "...";
                }

                if (entityDto.Type >= 1 || entityDto.Type <= 3)
                {
                    entityDto.Category = GetCategory((PostType)entityDto.Type);
                }

                var postEntity = _mapper.Map<PostEntity>(entityDto);
                var result = _postService.Create(postEntity);

                return Ok(new PostWebModelResponse());
               
            }
            catch(Exception ex)
            {
                var error = ErrorCode.ErrorCreatePost;
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
        public IActionResult Update([FromBody] PostEntity entity)
        {
            return Ok(_postService.Create(entity));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromBody] PostEntity entity)
        {
            return Ok(_postService.Create(entity));
        }

        private string GetCategory(PostType type)
        {
            return type.GetDescription();
        }
    }
}
