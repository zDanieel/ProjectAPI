using System.Collections.Generic;

namespace API.Models
{
    public class PostWebModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public string Category { get; set; }
        public int CustomerId { get; set; }
    }
}
