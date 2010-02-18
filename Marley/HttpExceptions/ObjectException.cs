using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class ObjectException : ApiResponseException
    {
        public ObjectException(IApiContext context)
            : base(context, string.Empty)
        {
        }
    }
}