using System;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;

namespace Marley.Contributors
{
    public class RequestExecutorContributor : IPipelineContributor
    {
        #region IPipelineContributor Members

        public void Register(IPipelineConfiguration context)
        {
            context.RegisterAfter<RequestBuilderContributor>(this);
        }

        public PipelineContinuation Execute(IApiContext context)
        {
            return PipelineContinuation.Continue;
        }

        #endregion
    }
}