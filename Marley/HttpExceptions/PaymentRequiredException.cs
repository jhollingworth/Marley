using System;
using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class PaymentRequiredException : ApiResponseException
    {
        public PaymentRequiredException(IApiContext context)
            : base(context, string.Empty)
        {
        }
    }
}