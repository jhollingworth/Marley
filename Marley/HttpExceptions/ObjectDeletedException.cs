using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class ObjectDeletedException : ApiResponseException
    {
        public ObjectDeletedException(IApiContext context)
            : base(context, string.Empty)
        {
        }
    }
}