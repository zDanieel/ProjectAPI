using System.Collections.Generic;

namespace DataAccess.Data
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
