using System.Collections.Generic;
using Marley.Contributors.Codecs;

namespace Marley.Pipeline.Configuration
{
    public interface IPipelineConfiguration
    {
        void RegisterBefore<TContributor>(IPipelineContributor contributor)
            where TContributor : IPipelineContributor;

        void RegisterAfter<TContributor>(IPipelineContributor contributor)
            where TContributor : IPipelineContributor;

        void RegisterContributor<TContributor>()
            where TContributor : IPipelineContributor, new();

        void RemoveContributor<TContributor>()
            where TContributor : IPipelineContributor;

        void RegisterCodec<TCodec>()
            where TCodec : ICodec, new();

        void RemoveCodec<TCodec>()
            where TCodec : ICodec;

        ICodec GetCodec(string contentType);
        List<ICodec> Codecs { get; }
        List<IPipelineContributor> Contributors { get; }
        IPipeline GetPipeline();
    }
}