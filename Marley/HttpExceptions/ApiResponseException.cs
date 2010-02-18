using System;
using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class ApiResponseException : Exception
    {
        public IApiContext Context { get; set; }

        public ApiResponseException(IApiContext context, string message, params object[] args)
            :base(string.Format(message, args))
        {
            Context = context;
        }
    }
}