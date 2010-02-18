using System;
using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class ConflictException : ApiResponseException
    {
        public ConflictException(IApiContext context)
            : base(context, string.Empty)
        {
        }
    }
}