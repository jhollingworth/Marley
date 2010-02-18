using Marley.Contributors;
using Marley.Contributors.Codecs;

namespace Marley.Pipeline.Configuration
{
    public class DefaultConfiguration : IConfiguration
    {
        #region IConfiguration Members

        public void Configure(IPipelineConfiguration pipeline, IResourceSpaceConfiguration resourceSpace)
        {
            pipeline.RegisterContributor<RequestExecutorContributor>();
            pipeline.RegisterContributor<RequestExecutorContributor>();

            pipeline.RegisterCodec<XmlCodec>();

            resourceSpace.Has<User>()
                .Uri("/Users")
                .ContentType("application/json");
        }

        #endregion

        private void RegisterCodecs(IPipelineConfiguration pipeline)
        {
            
        }

        private class User
        {
            
        }
    }
}