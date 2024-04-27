
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos
{
    public class CustomerDTO
    {
        [Required(ErrorMessage = "The customer name is required.")]
        [StringLength(500, ErrorMessage = "The customer name cannot be more than 500 characters.")]
        [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "The customer name is not valid.")]
        public string Name { get; set; }
    }
}
