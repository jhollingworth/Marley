using Marley.Pipeline.Context;

namespace Marley
{
    public interface IApiCall
    {
        IResponse Get(string uri);
        IResponse Post(string uri, string data);
        IResponse Execute(string uri, string data, object method);
    }
}