using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;

namespace Marley.Pipeline
{
    public interface IPipelineContributor
    {
        void Register(IPipelineConfiguration pipeline);
        PipelineContinuation Execute(IApiContext context);
    }
}