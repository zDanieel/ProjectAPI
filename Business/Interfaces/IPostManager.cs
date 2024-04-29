using Business.Dtos;
using DataAccess.Data;

namespace Business.Interfaces
{
    public interface IPostManager : IBaseService<Post>
    {
        Post CreatePost(PostDTO postDto);
        (Post post, bool changed) UpdatePost(Post postEntity);
    }
}
