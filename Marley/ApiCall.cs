using Marley.Pipeline;
using Marley.Pipeline.Context;
using log4net;

namespace Marley
{
    public class ApiCall : IApiCall
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (ApiCall));
        private readonly IPipeline _pipeline;

        public ApiCall(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        public IResponse Get(string uri)
        {
            return Execute(uri, null, "GET");
        }

        public IResponse Post(string uri, string data)
        {
            return Execute(uri, data, "POST");
        }

        public IResponse Execute(string uri, string data, object method)
        {
            _log.InfoFormat("Starting Call: {0} - {1}", uri, method);

            var context = new ApiContext
                              {
                                  Request = new ApiRequest {Uri = uri, Data = data, HttpMethod = method.ToString()}
                              };

            _log.Info(context.ToString());

            _pipeline.Execute(context);

            return context.Response;
        }
    }
}