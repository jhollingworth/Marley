using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;
using log4net;

namespace Marley
{
    public class ApiCall : IApiCall
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (ApiCall));
        private readonly IPipeline _pipeline;
        private readonly IResourceSpaceConfiguration _resourceSpaceConfiguration;
        private readonly IPipelineConfiguration _pipelineConfiguration;

        public ApiCall(IPipeline pipeline, IResourceSpaceConfiguration resourceSpaceConfiguration)
        {
            _pipeline = pipeline;
            _resourceSpaceConfiguration = resourceSpaceConfiguration;
        }

        public IResponse Get<TResource>(string uri)
        {
            return Execute<TResource>(uri, null, "GET");
        }

        public IResponse Post<TResource>(string uri, object resource)
        {
            return Execute<TResource>(uri, resource, "POST");
        }

        public IResponse Execute<TResource>(string uri, object resource, object method)
        {
            _log.InfoFormat("Starting Call: {0} - {1}", uri, method);

            var context = new ApiContext(
                new ApiRequest {Uri = uri, HttpMethod = method.ToString(), Data = resource},
                new ApiResponse(typeof(TResource))
            );

            _log.Info(context.ToString());
            _pipeline.Execute(context);

            if(context.PipelineException != null)
                throw context.PipelineException;

            return context.Response;
        }
    }
}