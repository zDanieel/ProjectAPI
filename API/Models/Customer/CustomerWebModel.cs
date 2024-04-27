using System.Collections.Generic;

namespace API.Models.Customer
{
    public class CustomerWebModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CustomerWebModelResponse : GenericResponse
    {
        public string MessangeInfo { get; set; } = string.Empty;
        public IEnumerable<CustomerWebModel> Customers { get; set; }
    }
}
