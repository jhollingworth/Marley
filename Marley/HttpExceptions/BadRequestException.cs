using System;
using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class BadRequestException : ApiResponseException
    {
        public BadRequestException(IApiContext context) : base(context, string.Empty)
        {
        }
    }
}