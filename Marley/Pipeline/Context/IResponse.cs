using System.Net;

namespace Marley.Pipeline.Context
{
    public interface IResponse
    {
        HttpWebResponse Response { get; set; }
        string Data { get; set; }
    }
}