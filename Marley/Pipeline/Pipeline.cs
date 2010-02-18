using System;
using System.Collections.Generic;
using System.Linq;
using Marley.Contributors.Codecs;
using Marley.Pipeline.Context;

namespace Marley.Pipeline
{
    public class Pipeline : IPipeline
    {
        public List<ICodec> Codecs { get; private set; }
        public List<IPipelineContributor> Contributors { get; private set; }

        public Pipeline(IEnumerable<IPipelineContributor> contributors, IEnumerable<ICodec> codecs)
        {
            Codecs = codecs.ToList();
            Contributors = contributors.ToList();
        }

        public void Execute(IApiContext context)
        {
            foreach (var contributor in Contributors)
            {
                try
                {
                    if(contributor.Execute(context) == PipelineContinuation.Abort)
                        break;
                }
                catch (Exception ex)
                {
                    context.PipelineException = ex;
                    break;
                }
            }
        }
    }
}