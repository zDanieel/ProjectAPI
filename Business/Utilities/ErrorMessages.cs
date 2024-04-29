using System.Collections.Generic;

namespace Business.Utilities
{
    public static class ErrorMessages
    {
        private static readonly Dictionary<int, string> ErrorMessageDictionary = new Dictionary<int, string>
        {
            { 1, "Error find customers" },
            { 2, "Error creating customer" },
            { 3, "Error update customer" },
            { 4, "Error delete customer" },
            { 5, "Error find posts" },
            { 6, "Error creating post" },
            { 7, "Error update post" },
            { 8, "Error delete post" },
            { 9, "Error creating multiple posts" }
        };

        public static string GetMessage(int errorCode)
        {
            return ErrorMessageDictionary.ContainsKey(errorCode) ? ErrorMessageDictionary[errorCode] : "Unknown error";
        }
    }
}
