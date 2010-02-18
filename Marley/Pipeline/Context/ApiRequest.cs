using System;
using System.Collections.Generic;
using System.Net;

namespace Marley.Pipeline.Context
{
    public class ApiRequest : IRequest
    {
        #region IRequest Members

        public string ContentType { get; set; }
        public string HttpMethod { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Uri { get; set; }
        public int Timeout { get; set; }
        public object Data { get; set; }
        public WebRequest Request { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        #endregion
    }
}