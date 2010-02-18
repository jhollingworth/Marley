using System;
using System.Net;
using Marley.Pipeline.Context;

namespace Marley.HttpExceptions
{
    public class NetworkConnectionException  : ApiResponseException
    {
        public NetworkConnectionException(IApiContext context)
            : base(context, string.Empty)
        {
        }
    }
}