using Marley.Pipeline.Context;

namespace Marley
{
    public interface IApiCall
    {
        IResponse Get<T>(string uri);
        IResponse Post<T>(string uri, object data);
        IResponse Execute<T>(string uri, object data, object method);
    }
}