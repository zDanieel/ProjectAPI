

using Business.Dtos;
using DataAccess.Data;

namespace Business.Interfaces
{
    public interface IPostService : IBaseService<Post>
    {
        Post CreatePost(PostDTO postDto);
    }
}
