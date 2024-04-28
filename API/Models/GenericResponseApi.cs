using System;
using System.Collections.Generic;

namespace API.Models
{
    public class GenericResponseApi<T>
    {
        public GenericResponseApi()
        {
            ErrorCode = "";
            ErrorMessage = "";
        }

        public bool Succes { get; set; }
        public string ErrorCode { get; set; } 
        public string ErrorMessage { get; set; }
        public string MessangeInfo { get; set; } 
        public long ElementsCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
