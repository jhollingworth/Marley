using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class ObjectNotFoundException : ApiResponseException
    {
        public ObjectNotFoundException(IApiContext context) : base(context, string.Empty)
        {
        }
    }
}