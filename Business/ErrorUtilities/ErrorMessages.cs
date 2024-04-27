
using System.Collections.Generic;

namespace Business.ErrorUtilities
{
    public static class ErrorMessages
    {
        private static readonly Dictionary<int, string> ErrorMessageDictionary = new Dictionary<int, string>
        {
            { 1, "Error find customers" },
            { 2, "Error creating customer" },
            { 3, "Error update customer" },
            { 4, "Error delete customer" }
        };

        public static string GetMessage(int errorCode)
        {
            return ErrorMessageDictionary.ContainsKey(errorCode) ? ErrorMessageDictionary[errorCode] : "Unknown error";
        }
    }
}
