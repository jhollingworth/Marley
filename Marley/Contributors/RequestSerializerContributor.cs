using System;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;

namespace Marley.Contributors
{
    public class RequestSerializerContributor : IPipelineContributor, IRequiresResourceSpaceConfiguration
    {
        private IResourceSpaceConfiguration _resourceSpaceConfig;
        private IPipelineConfiguration _pipeline;

        public void Register(IPipelineConfiguration pipeline)
        {
            _pipeline = pipeline;
            pipeline.RegisterBefore<RequestExecutorContributor>(this);
        }

        public PipelineContinuation Execute(IApiContext context)
        {
            if(context.Request.Data != null && context.Request.Data.GetType() != typeof(string))
            {
                var metadata = _resourceSpaceConfig.Get(context.Request.Data.GetType());
                var codec = _pipeline.GetCodec(metadata.ContentType);

                context.Request.Data = codec.Encode(context.Request.Data);
            }

            return PipelineContinuation.Continue;
        }

        public IResourceSpaceConfiguration ResourceSpaceConfiguration
        {
            set { _resourceSpaceConfig = value; }
        }
    }
}