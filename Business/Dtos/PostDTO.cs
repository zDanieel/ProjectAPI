
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos
{
    public class PostDTO
    {
        [StringLength(500, ErrorMessage = "The post Title cannot be more than 500 characters.")]
        public string Title { get; set; }
        [StringLength(500, ErrorMessage = "The post Body cannot be more than 500 characters.")]
        public string Body { get; set; }
        public int Type { get; set; }
        [StringLength(500, ErrorMessage = "The post Category cannot be more than 500 characters.")]
        public string Category { get; set; }
        public int CustomerId { get; set; }
    }
}
