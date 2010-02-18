using System.Collections.Generic;
using System.Net;

namespace Marley.Pipeline.Context
{
    public interface IRequest
    {
        string HttpMethod { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Uri { get; set; }
        int Timeout { get; set; }
        object Data { get; set; }
        WebRequest Request { get; set; }
        Dictionary<string, string> Headers { get; set; }
    }
}