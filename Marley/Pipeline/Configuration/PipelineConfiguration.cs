using System;
using System.Collections.Generic;
using System.Linq;
using Marley.Contributors.Codecs;
using Marley.Tests;

namespace Marley.Pipeline.Configuration
{
    public class PipelineConfiguration : IPipelineConfiguration
    {
        private readonly IResourceSpaceConfiguration _configuration;

        public PipelineConfiguration(IResourceSpaceConfiguration configuration)
        {
            _configuration = configuration;
            Codecs = new List<ICodec>();
            Metadata = new RegistrationMetadata();
            Contributors = new List<IPipelineContributor>();
        }

        public List<ICodec> Codecs { get; private set; }
        public RegistrationMetadata Metadata { get; private set; }
        public List<IPipelineContributor> Contributors { get; private set; }
        
        public IPipeline GetPipeline()
        {
            return new Pipeline(Contributors, Codecs);
        }

        public void RegisterBefore<TOtherContributor>(IPipelineContributor contributor) where TOtherContributor : IPipelineContributor
        {
            AddRegistrationMetadata<TOtherContributor>(contributor, RegistrationMetadata.RegistrationType.Before);
        }

        public void RegisterAfter<TOtherContributor>(IPipelineContributor contributor) where TOtherContributor : IPipelineContributor
        {
            AddRegistrationMetadata<TOtherContributor>(contributor, RegistrationMetadata.RegistrationType.After);
        }

        private void AddRegistrationMetadata<TOtherContributor>(IPipelineContributor contributor, RegistrationMetadata.RegistrationType registrationType)
        {
            Metadata.Add(new RegistrationMetadata.Registration { Contributor = contributor, OtherContributorType = typeof(TOtherContributor), Type = registrationType });
        }

        public void RegisterContributor<TContributor>()
            where TContributor : IPipelineContributor, new()
        {
            if (false == Contributors.Any(c => c is TContributor))
            {
                var contributor = new TContributor();

                if(contributor is IRequiresResourceSpaceConfiguration)
                {
                    ((IRequiresResourceSpaceConfiguration) contributor).ResourceSpaceConfiguration = _configuration;
                }

                Contributors.Add(contributor);
            }
        }

        public void RemoveContributor<TContributor>()
            where TContributor : IPipelineContributor
        {
            Contributors.RemoveAll(c => c is TContributor);
        }

        public void RegisterCodec<TCodec>()
            where TCodec : ICodec, new()
        {
            if (false == Codecs.Any(c => c is TCodec))
                Codecs.Add(new TCodec());
        }

        public void RemoveCodec<TCodec>()
            where TCodec : ICodec
        {
            Codecs.RemoveAll(c => c is TCodec);
        }

        public ICodec GetCodec(string contentType)
        {
            return Codecs.FirstOrDefault(c => string.Compare(c.ContentType, contentType, true) == 0);
        }
    }
}