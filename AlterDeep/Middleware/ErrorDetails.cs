using System;

namespace AlterDeep.Middleware
{
    [Serializable]
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}