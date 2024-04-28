using System.Collections.Generic;

namespace API.Models.Post
{
    public class PostWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PostWebModelResponse : GenericResponse
    {
        public IEnumerable<PostWebModel> Posts { get; set; }
    }
}
