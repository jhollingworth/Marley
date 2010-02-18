namespace Marley.Pipeline.Configuration
{
    public interface IConfiguration
    {
        void Configure(IPipelineConfiguration pipeline, IResourceSpaceConfiguration resourceSpace);
    }
}