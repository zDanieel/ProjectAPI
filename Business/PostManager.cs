using AutoMapper;
using Business.Dtos;
using Business.Interfaces;
using Business.Utilities.Enum;
using DataAccess.Data;
using DataAccess.Interfaces;

namespace Business
{
    public class PostManager : BaseService<Post>, IPostManager
    {
        private readonly IMapper _mapper;

        public PostManager(IBaseModel<Post> baseModel, IMapper mapper) : base(baseModel)
        {
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

        public (Post post, bool changed) UpdatePost(Post postEntity)
        {
            return (Update(postEntity.PostId, postEntity, out bool changed), changed);
        }

        private string GetCategory(PostType type)
        {
            return type.ToString();
        }
    }
}
