using Marley.Pipeline.Configuration;

namespace Marley.ResourceSpace
{
    public class ResourceSpaceFactory : IResourceSpaceFactory
    {
        private readonly IPipelineConfiguration _pipelineConfiguration;
        private readonly IResourceSpaceConfiguration _resourceSpaceConfiguration;

        public ResourceSpaceFactory(IConfiguration configuration)
        {
            _resourceSpaceConfiguration = new ResourceSpaceConfiguration();
            _pipelineConfiguration = new PipelineConfiguration(_resourceSpaceConfiguration);
            

            configuration.Configure(_pipelineConfiguration, _resourceSpaceConfiguration);
        }

        public IResourceSpace GetResourceSpace()
        {
            var pipeline = _pipelineConfiguration.GetPipeline();
            return new ResourceSpace(pipeline, _pipelineConfiguration, _resourceSpaceConfiguration);
        }
    }
}