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
        public IEnumerable<CustomerWebModel> Customers { get; set; }
    }
}
