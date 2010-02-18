using System.Net;

namespace Marley.Pipeline.Context
{
    public interface IResponse
    {
        WebResponse Response { get; set; }
        string Data { get; set; }
    }
}