using System.Collections.Generic;
using Marley.Contributors.Codecs;
using Marley.Pipeline.Context;

namespace Marley.Pipeline
{
    public interface IPipeline
    {
        List<ICodec> Codecs { get; }
        List<IPipelineContributor> Contributors { get; }
        void Execute(IApiContext context);
    }
}