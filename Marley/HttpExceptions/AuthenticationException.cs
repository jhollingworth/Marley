using System;
using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class AuthenticationException  : ApiResponseException
    {
        public AuthenticationException(IApiContext context) : base(context, string.Empty)
        {
        }
    }
}