using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class AuthorizationException : ApiResponseException
    {
        public AuthorizationException(IApiContext context) : base(context, "Authorization Exception occured")
        {
        }
    }
}