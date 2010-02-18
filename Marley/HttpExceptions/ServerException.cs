using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class ServerException : ApiResponseException
    {
        public ServerException(IApiContext context, string message, params object[] args) : base(context, string.Empty)
        {
        }
    }
}