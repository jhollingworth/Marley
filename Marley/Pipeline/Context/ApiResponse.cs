using System;
using System.Net;

namespace Marley.Pipeline.Context
{
    public class ApiResponse : IResponse
    {
        public ApiResponse(Type resourceType)
        {
            ResourceType = resourceType;
        }

        #region IResponse Members

        public HttpWebResponse Response { get; set; }
        public string Data { get; set; }
        public Type ResourceType { get; set; }

        #endregion
    }
}