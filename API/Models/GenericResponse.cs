using System;

namespace API.Models
{
    public class GenericResponse
    {
        public GenericResponse()
        {
            ErrorCode = "";
            ErrorMessage = "";
        }

        public bool Succes { get; set; }
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public long ElementsCount { get; set; }
    }
}
