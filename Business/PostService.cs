using AutoMapper;
using Business.Dtos;
using Business.Interfaces;
using Business.Utilities.Enum;
using DataAccess;
using DataAccess.Data;
using System;

namespace Business
{
    public class PostService : BaseService<Post>, IPostService
    {
        private readonly IBaseService<Customer> _customerService;
        private readonly IMapper _mapper;

        public PostService(BaseModel<Post> baseModel, IBaseService<Customer> customerService, IMapper mapper) : base(baseModel)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public Post CreatePost(PostDTO postDto)
        {
            if (!string.IsNullOrEmpty(postDto.Body) && postDto.Body.Length > 20)
            {
                postDto.Body = postDto.Body.Substring(0, 97) + "...";
            }

            if (postDto.Type >= 1 || postDto.Type <= 3)
            {
                postDto.Category = GetCategory((PostType)postDto.Type);
            }

            var postEntity = _mapper.Map<Post>(postDto);

            return Create(postEntity);
        }

        private string GetCategory(PostType type)
        {
            return type.ToString();
        }
    }
}
